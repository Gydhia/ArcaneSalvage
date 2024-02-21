using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanaSalvage.Equipment;
using TMPro;
using UnityEngine;

namespace ArcanaSalvage.UI
{
    public class UIMenu_Equipment : UIMenu
    {
        public UIEquipmentPopup EquipmentPopup;

        public List<UIEquipmentSlot> EquipmentSlots;

        [SerializeField] private UIEquipmentItem m_equipmentItemPrefab;
        [SerializeField] private RectTransform m_inventoryHolder;

        [SerializeField] private TextMeshProUGUI m_attackTotal;
        [SerializeField] private TextMeshProUGUI m_healthTotal;
        
        public UIEquipmentItem EquipmentItemPrefab => m_equipmentItemPrefab;

        private Dictionary<Equipment.Equipment, UIEquipmentSlot> m_equipped = new Dictionary<Equipment.Equipment, UIEquipmentSlot>();
        private List<UIEquipmentItem> m_inventory = new List<UIEquipmentItem>();

        private void OnValidate()
        {
            m_equipped = new Dictionary<Equipment.Equipment, UIEquipmentSlot>();
            foreach (var slot in EquipmentSlots)
            {
                m_equipped.Add(slot.EquipmentType, slot);
            }
        }

        private void Start()
        {
            PlayerData.CurrentPlayerData.OnItemEquipped += AttachItemToSlot;
            PlayerData.CurrentPlayerData.OnItemUnequipped += UnequipItem;
            PlayerData.CurrentPlayerData.OnItemSold += OnItemSold;

            var inventory = PlayerData.CurrentPlayerData.GetPlayerInventory();
            var equipped = PlayerData.CurrentPlayerData.GetPlayerEquipped();
            
            for (int i = 0; i < inventory.Count; i++)
            {
                CreateInventoryItem(inventory[i]);
            }
            
            for (int i = 0; i < equipped.Count; i++)
            {
                if (equipped[i] != null)
                {
                    m_equipped[equipped[i].Slot].SetItem(equipped[i]);
                }
            }
            
            UpdateStatistics();
        }

        private void AttachItemToSlot(UIEquipmentItem item)
        {
            var fittingSlot = EquipmentSlots.Single(s => s.EquipmentType == item.EquipPreset.Slot);
            fittingSlot.SetItem(item.EquipPreset);

            m_inventory.Remove(item);
            Destroy(item.gameObject);

            UpdateStatistics();
        }

        public void UnequipItem(Equipment.Equipment slot, EquipmentPreset preset)
        {
            m_equipped[slot].SetItem(null);
            
            CreateInventoryItem(preset);

            UpdateStatistics();
        }

        public void OnItemSold(UIEquipmentItem item)
        {
            m_inventory.Remove(item);
            Destroy(item.gameObject);
        }
        
        private void UpdateStatistics()
        {
            m_attackTotal.text = "+" + PlayerData.CurrentPlayerData.GetAttackOverride();
            m_healthTotal.text = "+" + PlayerData.CurrentPlayerData.GetHealthOverride();
        }

        private void CreateInventoryItem(EquipmentPreset preset)
        {
            var invItem = Instantiate(m_equipmentItemPrefab, m_inventoryHolder);
            invItem.Init(preset);

            m_inventory.Add(invItem);
        }
        
        private void OnDestroy()
        {
            PlayerData.CurrentPlayerData.OnItemEquipped -= AttachItemToSlot;
            PlayerData.CurrentPlayerData.OnItemUnequipped -= UnequipItem;
        }
    }
}