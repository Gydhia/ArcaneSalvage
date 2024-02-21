using System;
using System.Collections.Generic;
using ArcanaSalvage;
using UnityEngine;
using ArcanaSalvage.UI;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_refreshCost;
    [SerializeField] private Button m_refreshButton;

    public static UpgradeSystem Instance { get; private set; }

    public const int REFRESH_COST = 50;
    
    public CardManager cardManager;
    public List<CardInfo> listOfCard;
    public int nbrOfCardToDisplay = 3;
    public Transform cardUIParent;

    public GameObject visual;

    private void Awake()
    {
        visual.SetActive(false);
        listOfCard = cardManager.cardInfos;
    }

    private void Start()
    {
        ActualizeRefreshState();
    }

    public void DisplayCardMenu()
    {
        List<CardInfo> pickedCards = PickRandomCards(nbrOfCardToDisplay);
        DisplayListOfCard(true, pickedCards);
    }

    private List<CardInfo> PickRandomCards(int count)
    {
        List<CardInfo> pickedCards = new List<CardInfo>();
        List<CardInfo> copyOfCards = new List<CardInfo>(listOfCard);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, copyOfCards.Count);
            pickedCards.Add(copyOfCards[randomIndex]);
            copyOfCards.RemoveAt(randomIndex);
        }
        
        return pickedCards;
    }


    public void OnClickRefresh()
    {
        PlayerData.CurrentPlayerData.ModifyGolds(-REFRESH_COST);

        ActualizeRefreshState();
        
        DisplayCardMenu();
    }

    private void ActualizeRefreshState()
    {   
        bool affordable = PlayerData.CurrentPlayerData.GetGolds() >= REFRESH_COST;

        m_refreshCost.text = REFRESH_COST.ToString();
        m_refreshCost.color = affordable ? Color.white : Color.red;
        m_refreshButton.interactable = affordable;
    }

    private void DisplayListOfCard(bool displayBool, List<CardInfo> cards = null)
    {
        foreach (CardInfo card in listOfCard)
        {
            bool shouldDisplay = (cards == null || cards.Contains(card));

            GameObject cardUI = FindCardUI(card);

            if (cardUI != null)
            {
                cardUI.SetActive(shouldDisplay && displayBool);
            }
        }
    }

    private GameObject FindCardUI(CardInfo card)
    {
        if (cardUIParent == null)
        {
            Debug.LogError("cardUIParent n'est pas assigné");
            return null;
        }

        foreach (Transform child in cardUIParent)
        {
            UIUpgrade uiUpgrade = child.GetComponent<UIUpgrade>();

            if (uiUpgrade != null)
            {
                CardInfo uiCard = uiUpgrade.GetCardInfo();

                if (uiCard != null && uiCard == card)
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }


    public void OpenMenu()
    {
        visual.SetActive(true);
        DisplayCardMenu();
    }

    public void CloseMenu()
    {
        visual.SetActive(false);
    }



    private void ShuffleCopy<T>(List<T> listToShuffle)
    {
        System.Random rng = new System.Random();
        int n = listToShuffle.Count;
        while(n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (listToShuffle[k], listToShuffle[n]) = (listToShuffle[n], listToShuffle[k]);
        }
    }
}
