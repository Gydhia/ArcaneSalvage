using System.Collections;
using ArcanaSalvage;
using Assets.Code.Scripts.Game.Player;
using Unity.Entities;
using UnityEngine;

namespace Code.Scripts.Game.Player
{
    public class LevelSystem : MonoBehaviour
    {
        public int currentLevel = 1;
        public float currentXP = 0f;
        public float xpRequiredMultiplier = 1.5f;
        public float xpGainMultiplier = 10.0f;
        public float requiredXPForNextLevel;

        public UpgradeSystem upgradeSystem;

        public int LastKillCounter = 0;
        
        public void GainXP(float xpAmount, float xpGainMultiplicator = float.MaxValue)
        {
            if (xpGainMultiplicator == float.MaxValue)
                xpGainMultiplicator = xpGainMultiplier;
            
            xpGainMultiplier = xpGainMultiplicator;
            currentXP += xpAmount * xpGainMultiplier;
            CheckForLevelUp();
        }

        public float CalculateRequiredXPForNextLevel()
        {
            requiredXPForNextLevel = Mathf.Pow(currentLevel, xpRequiredMultiplier) * 100;
            return requiredXPForNextLevel;
        }

        private void CheckForLevelUp()
        {
            if (currentXP >= CalculateRequiredXPForNextLevel())
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            requiredXPForNextLevel = CalculateRequiredXPForNextLevel();
            upgradeSystem.OpenMenu();
            if (currentXP >= requiredXPForNextLevel)
            {
                currentLevel++;
                currentXP -= requiredXPForNextLevel;
            }
        }
    }
}
