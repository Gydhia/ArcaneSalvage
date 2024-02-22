using UnityEngine;
using ArcanaSalvage.UI;

public enum UpgradeType
{
    Angle,
    Damage,
    Exploding,
    FireRate,
    MoreArrow,
    Piercing,
    MoreRange
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card Info")]
public class CardInfo : ScriptableObject
{
    public UpgradeType upgradeType;
    public int modifier;

    public Sprite icon;
    public string nameOfCard;
    public string description;
    public int price;

    public int nbrOfTimeHasBeenPucharsed = 0;
    public bool canBePurchasedMultiple;

}
