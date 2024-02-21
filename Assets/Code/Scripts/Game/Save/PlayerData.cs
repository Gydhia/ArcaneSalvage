using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanaSalvage.Equipment;
using UnityEngine;

namespace ArcanaSalvage
{
    public class PlayerDataSave
    {
        public List<Guid> PlayerItemsInventory;
        public List<Guid> PlayerItemsEquipped;

        public void Save(PlayerData pData)
        {
            PlayerItemsInventory = pData.GetPlayerInventory().Select(i => i.UID).ToList();
            PlayerItemsEquipped = pData.GetPlayerEquipped().Select(i => i.UID).ToList();
        }
    }
    
    public class PlayerData
    {
        public PlayerData()
        {
            m_playerItemsEquipped = new List<EquipmentPreset>();
            m_playerItemsInventory = new List<EquipmentPreset>();

            m_playerSave = new PlayerDataSave();
        }
        
        private PlayerDataSave m_playerSave;
        
        // Runtime variables
        private List<EquipmentPreset> m_playerItemsInventory;
        private List<EquipmentPreset> m_playerItemsEquipped;
        
        private int m_attackOverride;
        private int m_moveSpeedOverride;
        private int m_healthOverride;

        public void RefreshOverrides()
        {
            m_healthOverride = m_playerItemsEquipped.Sum(i => i.HealthModifier);
            m_attackOverride = m_playerItemsEquipped.Sum(i => i.AttackModifier);
            m_healthOverride = m_playerItemsEquipped.Sum(i => i.MoveSpeedModifier);

            m_playerSave.Save(this);
        }

        public void EquipItem(EquipmentPreset preset)
        {
            m_playerItemsInventory.Remove(preset);
            m_playerItemsEquipped.Add(preset);
            
            m_playerSave.Save(this);
        }

        public List<EquipmentPreset> GetPlayerEquipped() => m_playerItemsEquipped;
        public List<EquipmentPreset> GetPlayerInventory() => m_playerItemsInventory;

        
        public int GetHealthOverride() => m_healthOverride;
        
        public int GetAttackOverride() => m_attackOverride;
        
        public int GetMoveSpeedOverride() => m_moveSpeedOverride;
        
        public PlayerDataSave GetPlayerSave() => m_playerSave;

    }
}
