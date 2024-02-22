using UnityEngine;
using UnityEngine.UI;
using Code.Scripts.Game.Player;
using TMPro;


public class LevelUI : MonoBehaviour
{
    public LevelSystem levelSystem;
    public TMP_Text levelText;
    public Slider xpBarImage;

    private void Start()
    {
        xpBarImage.value = 0;
    }

    void Update()
    {
        levelText.text = levelSystem.currentLevel.ToString();

        float fillAmount = levelSystem.currentXP / levelSystem.CalculateRequiredXPForNextLevel();
        xpBarImage.value = fillAmount;
    }
}