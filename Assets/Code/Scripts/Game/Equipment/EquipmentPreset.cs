using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaSalvage.Equipment
{
    public enum Equipment
    {
        None = 0,
    
        Staff = 1,
        Rune = 2,

        Helmet = 3,
        Chestplate = 4,
        Gloves = 5,
        Boots = 6,
    }

    [CreateAssetMenu(fileName = "EquipmentPreset",menuName = "Equipments/EquipmentPreset")]
    public class EquipmentPreset : ScriptableObject
    {
        public Guid UID;

        private void OnValidate()
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Name));
                this.UID = new Guid(hash);
            }
        }

        public string Name;
        public Sprite Icon;
    
        public string Description;

        public int AttackModifier;
        public int HealthModifier;
        public int MoveSpeedModifier;

        public int SellPrice;
    }
}