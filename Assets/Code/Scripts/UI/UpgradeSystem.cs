using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{


    public List<GameObject> listOfCard;
    public Transform horizontalLayoutObject;

    public int nbrOfCardToDisplay = 3;

    public List<GameObject> pickedCards = new List<GameObject>(); 

    private void Start()
    {
        foreach (var item in listOfCard)
        {
            GameObject instance = Instantiate(item, horizontalLayoutObject);
        }
    }
    private void Update()
    {
        foreach (var item in pickedCards)
        {
            item.SetActive(true);
        }
        foreach (var item in listOfCard)
        {
            item.SetActive(true);
        }
    }

    public void displayCardMenu()
    {
        pickedCards.Clear();
        displayListOfCard(listOfCard, false);

        List<GameObject> copieListOfCard = ShuffleCopy<GameObject>(listOfCard, new System.Random());
        if (listOfCard.Count > 0)
        {
            for (int i = 0; i < nbrOfCardToDisplay; i++)
            {
                int randomIndex = Random.Range(0, copieListOfCard.Count);
                pickedCards.Add(copieListOfCard[randomIndex]);
                copieListOfCard.RemoveAt(randomIndex);
            }
            foreach (var item in pickedCards)
            {
                Debug.Log(item);
            }
            displayListOfCard(pickedCards, true);
        }
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        displayCardMenu();
    }

    public void Refresh()
    {
        displayCardMenu();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    private void displayListOfCard(List<GameObject> listOfCard, bool displayBool)
    {
        foreach (GameObject item in listOfCard)
        {
            item.SetActive(displayBool);
        }
    }

    public List<T> ShuffleCopy<T>(List<T> list, System.Random generator)
    {
        List<T> copy = new List<T>(list);

        int n = copy.Count;
        while (n > 1)
        {
            n--;
            int k = generator.Next(n + 1);
            (copy[k], copy[n]) = (copy[n], copy[k]);
        }

        return copy;
    }
}
