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
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card Info")]
public class CardInfo : ScriptableObject
{
    public UpgradeType upgradeType;
    public Sprite icon;
    public string nameOfCard;
    public string description;
    public int price;

}
