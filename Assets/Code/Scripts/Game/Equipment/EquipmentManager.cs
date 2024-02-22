using System;
using System.Collections;
using System.Collections.Generic;
using ArcanaSalvage;
using UnityEngine;

namespace ArcanaSalvage.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentManager",menuName = "SingletonPreset/EquipmentManager")]
    public class EquipmentManager : SingletonScriptableObject<EquipmentManager>
    {
        [SerializeField] private List<EquipmentPreset> m_equipments;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            try{
                if (Application.isPlaying)
                    return;
                        
                EquipmentPresets = new Dictionary<Guid, EquipmentPreset>();
                for (int i = 0; i < m_equipments.Count; i++)
                {
                    EquipmentPresets.Add(m_equipments[i].UID, m_equipments[i]);
                }
            }
            catch{
                
            }
        }
        #endif
        
        public Dictionary<Guid, EquipmentPreset> EquipmentPresets;
        public List<EquipmentPreset> Equipments => m_equipments;

    }
}