using System.Collections;
using System.Collections.Generic;
using ArcanaSalvage.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaSalvage.UI
{
    public class UIEquipmentPopup : MonoBehaviour
    {
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_itemName;
        
        [SerializeField] private TextMeshProUGUI m_healthModifier;
        [SerializeField] private TextMeshProUGUI m_attackModifier;
        
        [SerializeField] private TextMeshProUGUI m_sellPrice;

        private EquipmentPreset m_currentPreset;
        

        public void Open(EquipmentPreset preset)
        {
            gameObject.SetActive(true);
            
            m_itemIcon.sprite = preset.Icon;
            m_itemName.text = preset.Name;

            m_healthModifier.text = preset.HealthModifier.ToString();
            m_attackModifier.text = preset.AttackModifier.ToString();

            m_sellPrice.text = preset.SellPrice.ToString();
        }

        public void OnClickSell()
        {
            
        }
        public void OnClickUpgrade()
        {
            
        }
        public void OnClickEquip()
        {
            gameObject.SetActive(false);
        }
    }
}