using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Helper;
using TMPro;
using UnityEngine;

namespace ArcanaSalvage.UI
{
    public class GameUIManager : Singleton<GameUIManager>
    {
        [SerializeField] private TextMeshProUGUI m_goldText;
        [SerializeField] private TextMeshProUGUI m_enemyKillsText;

        [SerializeField] private TextMeshProUGUI m_timer;

        [SerializeField] private RectTransform m_pauseSection;

        
        private float startTime;
        private void Start()
        {
            PlayerData.CurrentPlayerData.OnGoldsChanged += UpdateGolds;
            PlayerData.CurrentPlayerData.OnEnemyKillsChanged += UpdateEnemyKills;

            UpdateGolds(PlayerData.CurrentPlayerData.GetGolds());
            UpdateEnemyKills(PlayerData.CurrentPlayerData.GetEnemyKills());
                
            m_pauseSection.gameObject.SetActive(false);
            
            startTime = Time.time;
        }

        public void Update()
        {
            TimeSpan time = TimeSpan.FromSeconds(Time.time - startTime);

            m_timer.text = time.ToString(@"mm\:ss");
        }
        
        private void UpdateGolds(int nb)
        {
            m_goldText.text = nb.ToString();
        }
        
        private void UpdateEnemyKills(int nb)
        {
            m_enemyKillsText.text = nb.ToString();
        }

        public void OnPauseClicked()
        {
            m_pauseSection.gameObject.SetActive(true);
        }
        
        public void OnResumeClicked()
        {
            m_pauseSection.gameObject.SetActive(false);
        }

        public void OnReturnToMainMenuClicked()
        {
            SceneController.Instance.LoadScene("MainMenu");
        }
    }
}