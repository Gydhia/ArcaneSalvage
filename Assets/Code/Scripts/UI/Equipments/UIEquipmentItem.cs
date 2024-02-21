using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanaSalvage.Equipment;
using UnityEngine;

namespace ArcanaSalvage.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        public EquipmentPreset EquipPreset;

        public void Init(EquipmentPreset equipmentPreset)
        {
            EquipPreset = equipmentPreset;
        }

        public void OnClickItem()
        {
            var menu = MainMenuManager.Instance.GetMenu(MenuSection.Equipment) as UIMenu_Equipment;
            
            menu.EquipmentPopup.Open(EquipPreset);
        }
    }
}