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
        [SerializeField] private TextMeshProUGUI m_moveSpeedModifier;
        
        [SerializeField] private TextMeshProUGUI m_sellPrice;
        [SerializeField] private TextMeshProUGUI m_upgradePrice;

        [SerializeField] private Button m_sellButton;
        [SerializeField] private Button m_equipButton;
        [SerializeField] private Button m_upgradeButton;

        [SerializeField] private TextMeshProUGUI m_equipButtonText;
        [SerializeField] private TextMeshProUGUI m_upgradeButtonText;

        
        private UIEquipmentItem m_currentItem;
        

        public void Open(UIEquipmentItem item)
        {
            gameObject.SetActive(true);

            m_currentItem = item;
            
            m_itemIcon.sprite = item.EquipPreset.Icon;
            m_itemName.text = item.EquipPreset.Name;

            m_healthModifier.text = item.EquipPreset.HealthModifier.ToString();
            m_attackModifier.text = item.EquipPreset.AttackModifier.ToString();
            m_moveSpeedModifier.text = item.EquipPreset.MoveSpeedModifier.ToString();

            m_sellPrice.text = item.EquipPreset.SellPrice.ToString();
            m_upgradePrice.text = item.EquipPreset.UpgradePrice.ToString();

            bool affordable = PlayerData.CurrentPlayerData.GetGolds() >= item.EquipPreset.UpgradePrice;
            
            m_sellButton.interactable = !item.IsEquiped;
            m_upgradeButton.interactable = affordable;
            
            m_upgradeButtonText.color = affordable
                ? Color.white
                : Color.red;
            m_equipButtonText.text = item.IsEquiped ? "Unequip" : "Equip";
        }

        private void Close()
        {
            m_currentItem = null;
            gameObject.SetActive(false);
        }
        
        public void OnClickExit()
        {
            Close();
        }
        
        public void OnClickSell()
        {
            PlayerData.CurrentPlayerData.SellItem(m_currentItem);
            Close();
        }
        
        public void OnClickUpgrade()
        {
            
        }
        
        public void OnClickEquip()
        {
            if (PlayerData.CurrentPlayerData.IsFreeSlot(m_currentItem.EquipPreset.Slot))
            {
                PlayerData.CurrentPlayerData.EquipItem(m_currentItem);

                Close();
            }
            else
            {
                PlayerData.CurrentPlayerData.UnequipItem(m_currentItem);
                
                Close();
            }
        }
    }
}