using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcanaSalvage.Equipment;

namespace ArcanaSalvage.UI
{
    public class UIEquipmentSlot : MonoBehaviour
    {
        public Equipment.Equipment EquipmentType;

        public EquipmentPreset EquippedPreset;

        public UIEquipmentItem Item;

        private void Awake()
        {
            SetItem(null);
        }

        public void SetItem(EquipmentPreset preset)
        {
            EquippedPreset = preset;

            if (preset == null)
            {
                Item.IsEquiped = false;
                Item.gameObject.SetActive(false);
            }
            else
            {
                Item.IsEquiped = true;
                Item.gameObject.SetActive(true);
                Item.Init(preset);
            }
        }
    }
}