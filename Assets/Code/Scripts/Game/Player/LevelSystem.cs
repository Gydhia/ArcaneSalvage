using UnityEngine;

namespace Code.Scripts.Game.Player
{
    public class LevelSystem : MonoBehaviour
    {
        public int currentLevel = 1;
        public float currentXP = 0f;
        public float xpRequiredMultiplier = 1.5f;
        public float xpGainMultiplier = 1.0f;
        public float requiredXPForNextLevel;


        public void GainXP(float xpAmount, float xpGainMultiplicator)
        {
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

            if (currentXP >= requiredXPForNextLevel)
            {
                currentLevel++;
                currentXP -= requiredXPForNextLevel;
            }
        }
    }
}
