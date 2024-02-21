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


        public List<EquipmentPreset> Equipments => m_equipments;
    }
}