using UnityEngine;
namespace Code.Scripts.Game.Player { 
    public class LevelSystem : MonoBehaviour
    {
        public int CurrentLevel = 1;
        public float CurrentXP = 0f;
        public float XpRequiredMultiplier = 1.5f;
        public float XpGainMultiplier = 1.0f;

        private bool levelUpTriggered = false;

        public void GainXP(float xpAmount)
        {
            CurrentXP += xpAmount * XpGainMultiplier;

            if (!levelUpTriggered)
            {
                CheckForLevelUp();
            }
        }

        private float CalculateRequiredXPForNextLevel()
        {
            return Mathf.Pow(CurrentLevel, XpRequiredMultiplier) * 100;
        }

        private void CheckForLevelUp()
        {
            while (CurrentXP >= CalculateRequiredXPForNextLevel())
            {
                LevelUp();
                levelUpTriggered = true;
            }
        }

        private void LevelUp()
        {
            CurrentLevel++;
            CurrentXP -= CalculateRequiredXPForNextLevel();

            Debug.Log("Congratulations! You've reached level " + CurrentLevel);
            levelUpTriggered = false;
        }
    }
}