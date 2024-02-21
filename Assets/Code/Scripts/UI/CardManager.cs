using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaSalvage.UI;


public class CardManager : MonoBehaviour
{
    public GameObject cardUIPrefab; 
    public Transform cardUIParent;
    public List<CardInfo> cardInfos = new List<CardInfo>();

    private void Start()
    {
        DisplayCardUIs();
    }

    private void DisplayCardUIs()
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);

            UIUpgrade uiUpgrade = cardUI.GetComponent<UIUpgrade>();

            uiUpgrade.Init(cardInfo);
        }
    }
}
