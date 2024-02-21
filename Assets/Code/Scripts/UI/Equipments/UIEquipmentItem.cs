using ArcanaSalvage.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaSalvage.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        
        public EquipmentPreset EquipPreset;

        public bool IsEquiped;

        public void Init(EquipmentPreset equipmentPreset)
        {
            EquipPreset = equipmentPreset;

            if (equipmentPreset != null)
            {
                m_icon.sprite = equipmentPreset.Icon;
            }
        }

        public void OnClickItem()
        {
            var menu = MainMenuManager.Instance.GetMenu(MenuSection.Equipment) as UIMenu_Equipment;
            
            menu.EquipmentPopup.Open(this);
        }
    }
}