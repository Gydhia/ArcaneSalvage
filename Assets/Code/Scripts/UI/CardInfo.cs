using UnityEngine;
using ArcanaSalvage.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Info")]
public class CardInfo : ScriptableObject
{
    public Sprite icon;
    public string nameOfCard;
    public string description;
    public int price;

}
