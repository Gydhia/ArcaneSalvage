using System;
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

        [SerializeField] private RectTransform m_gameResultsSection;
        [SerializeField] private TextMeshProUGUI m_goldsResult;
        [SerializeField] private TextMeshProUGUI m_killsResults;

        [SerializeField] private TextMeshProUGUI m_resultText;

        private float startTime;

        private void Start()
        {
            PlayerData.CurrentPlayerData.OnGoldsChanged += UpdateGolds;
            PlayerData.CurrentPlayerData.OnEnemyKillsChanged += UpdateEnemyKills;

            UpdateGolds(PlayerData.CurrentPlayerData.GetGolds());
            UpdateEnemyKills(PlayerData.CurrentPlayerData.GetEnemyKills());

            m_gameResultsSection.gameObject.SetActive(false);
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
            Time.timeScale = 0f;
        }

        public void OnResumeClicked()
        {
            m_pauseSection.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        public void OnReturnToMainMenuClicked()
        {
            SceneController.Instance.LoadScene("MainMenu");
        }

        public void OnGameEnded(bool result)
        {
            m_gameResultsSection.gameObject.SetActive(true);
            
            m_resultText.text = result ? "VICTORY" : "DEFEAT";
            m_resultText.color = result ? Color.green : Color.red;

            m_goldsResult.text = PlayerData.CurrentPlayerData.GetGolds().ToString();
            m_killsResults.text = PlayerData.CurrentPlayerData.GetEnemyKills().ToString();
        }
    }
}