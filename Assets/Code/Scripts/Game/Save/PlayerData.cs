using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanaSalvage.Equipment;
using ArcanaSalvage.UI;
using UnityEngine;

namespace ArcanaSalvage
{
    /// <summary>
    /// SAVE Class for the player datas
    /// </summary>
    public class PlayerDataSave
    {
        public PlayerDataSave()
        {
            Load();
        }
        
        public List<Guid> PlayerItemsInventory;
        public List<Guid> PlayerItemsEquipped;

        public int Golds;
        public int EnemyKills;

        public void Save(PlayerData pData)
        {
            PlayerItemsInventory = pData.GetPlayerInventory().Select(i => i.UID).ToList();
            PlayerItemsEquipped = pData.GetPlayerEquipped().Where(i => i != null).Select(i => i.UID).ToList();
            
            SaveGuidList("PlayerInventory", PlayerItemsInventory);
            SaveGuidList("PlayerEquipped", PlayerItemsEquipped);
            
            SaveGoldsAndEnemies(pData);
        }

        public void SaveGoldsAndEnemies(PlayerData pData)
        {
            PlayerPrefs.SetString("Golds", Golds.ToString());
            PlayerPrefs.Save(); 
            
            PlayerPrefs.SetString("EnemyKills", EnemyKills.ToString());
            PlayerPrefs.Save(); 
        }

        public void Load()
        {
            PlayerItemsInventory = LoadGuidList("PlayerInventory");
            PlayerItemsEquipped = LoadGuidList("PlayerEquipped");

            int.TryParse(PlayerPrefs.GetString("Golds"), out Golds);
            int.TryParse(PlayerPrefs.GetString("EnemyKills"), out EnemyKills);
        }
        
        public void SaveGuidList(string key, List<Guid> guidList)
        {
            string serializedData = JsonUtility.ToJson(guidList);
            PlayerPrefs.SetString(key, serializedData);
            PlayerPrefs.Save(); 
        }

        public List<Guid> LoadGuidList(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string serializedData = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<List<Guid>>(serializedData);
            }
            else
            {
                Debug.LogWarning("No data found for key: " + key);
                return new List<Guid>();
            }
        }
    }
    
    /// <summary>
    /// RUNTIME Class for the player datas
    /// </summary>
    public class PlayerData
    {
        public static PlayerData CurrentPlayerData;
        
        public PlayerData()
        {
            m_playerItemsEquipped = new Dictionary<Equipment.Equipment, EquipmentPreset>();
            m_playerItemsInventory = new List<EquipmentPreset>();
            
            foreach (Equipment.Equipment slot in Enum.GetValues(typeof(Equipment.Equipment)))
                m_playerItemsEquipped.Add(slot, null);

            m_playerSave = new PlayerDataSave();

            foreach (var invItem in m_playerSave.PlayerItemsInventory)
            {
                m_playerItemsInventory.Add(EquipmentManager.Instance.EquipmentPresets[invItem]);
            }
            foreach (var equipItem in m_playerSave.PlayerItemsEquipped)
            {
                var item = EquipmentManager.Instance.EquipmentPresets[equipItem];
                m_playerItemsEquipped[item.Slot] = item;
            }

            m_golds = m_playerSave.Golds;
            m_enemyKills = m_playerSave.EnemyKills;

#if UNITY_EDITOR
            m_playerItemsInventory.AddRange(EquipmentManager.Instance.Equipments);
#endif
        }
        
        private PlayerDataSave m_playerSave;
        
        // Runtime variables
        private List<EquipmentPreset> m_playerItemsInventory;
        private Dictionary<Equipment.Equipment , EquipmentPreset> m_playerItemsEquipped;
        
        private int m_attackOverride;
        private int m_moveSpeedOverride;
        private int m_healthOverride;

        private int m_golds;
        private int m_enemyKills;

        public Action<UIEquipmentItem> OnItemEquipped;
        public Action<Equipment.Equipment, EquipmentPreset> OnItemUnequipped;
        public Action<UIEquipmentItem> OnItemSold;
        public Action<int> OnGoldsChanged;
        public Action<int> OnEnemyKillsChanged;
        
        public void RefreshOverrides()
        {
            m_healthOverride = m_playerItemsEquipped.Values.Where(i => i != null).Sum(i => i.HealthModifier);
            m_attackOverride = m_playerItemsEquipped.Values.Where(i => i != null).Sum(i => i.AttackModifier);
            m_moveSpeedOverride = m_playerItemsEquipped.Values.Where(i => i != null).Sum(i => i.MoveSpeedModifier);
            
            m_playerSave.Save(this);
        }

        public void EquipItem(UIEquipmentItem item)
        {
            m_playerItemsInventory.Remove(item.EquipPreset);
            m_playerItemsEquipped[item.EquipPreset.Slot] = item.EquipPreset;
            
            m_playerSave.Save(this);

            RefreshOverrides();

            OnItemEquipped?.Invoke(item);
        }

        public void UnequipItem(UIEquipmentItem item)
        {
            m_playerItemsEquipped[item.EquipPreset.Slot] = null;
            
            m_playerSave.Save(this);
            
            RefreshOverrides();
            
            OnItemUnequipped?.Invoke(item.EquipPreset.Slot, item.EquipPreset);
        }

        public void SellItem(UIEquipmentItem item)
        {
            m_playerItemsInventory.Remove(item.EquipPreset);
            
            ModifyGolds(item.EquipPreset.SellPrice);
            m_playerSave.Save(this);
            
            OnItemSold?.Invoke(item);
        }
        

        public void ModifyGolds(int amount)
        {
            m_golds += amount;

            m_playerSave.SaveGoldsAndEnemies(this);
            
            OnGoldsChanged?.Invoke(m_golds);
        }
        
        public List<EquipmentPreset> GetPlayerEquipped() => m_playerItemsEquipped.Values.ToList();
        public List<EquipmentPreset> GetPlayerInventory() => m_playerItemsInventory;
        public bool IsFreeSlot(Equipment.Equipment slot) => m_playerItemsEquipped[slot] == null;

        
        public int GetHealthOverride() => m_healthOverride;
        
        public int GetAttackOverride() => m_attackOverride;
        
        public int GetMoveSpeedOverride() => m_moveSpeedOverride;

        public int GetGolds() => m_golds;
        public int GetEnemyKills() => m_enemyKills;
        
        public PlayerDataSave GetPlayerSave() => m_playerSave;
    }
}
