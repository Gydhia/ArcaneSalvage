using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcanaSalvage.UI
{
    public class UIUpgrade : MonoBehaviour
    {
        [SerializeField] private Image m_upradeIcon;
        
        [SerializeField] private TextMeshProUGUI m_upgradeName;
        [SerializeField] private TextMeshProUGUI m_upgradeDescription;
        [SerializeField] private int m_price;

        [SerializeField] private Button m_upgradeButton;

        public Button UpgradeButton => m_upgradeButton;

        private CardInfo m_cardInfo;


        public void Init(CardInfo cardInfo)
        {
            if (cardInfo == null)
            {
                Debug.LogError("CardInfo est null.");
                return;
            }

            m_upradeIcon.sprite = cardInfo.icon;
            m_upgradeName.text = cardInfo.nameOfCard;
            m_upgradeDescription.text = cardInfo.description;
            m_price = cardInfo.price;
            m_cardInfo = cardInfo;

        }

        public CardInfo GetCardInfo()
        {
            return m_cardInfo;
        }
    }
}