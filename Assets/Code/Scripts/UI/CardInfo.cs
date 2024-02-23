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
    MoreRange,
    MoreSpeed
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card Info")]
public class CardInfo : ScriptableObject
{
    public UpgradeType upgradeType;
    public float modifier = 0.0f;

    public Sprite icon;
    public string nameOfCard;
    public string description;
    public int price;

    public int nbrOfTimeHasBeenPucharsed = 0;
    public bool canBePurchasedMultiple;

}
