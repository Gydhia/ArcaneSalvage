using System;
using System.Collections;
using Assets.Code.Scripts.Game.Player;
using Code.Scripts.Helper;
using TMPro;
using Unity.Entities;
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
        [SerializeField] private UnityEngine.UI.Image m_resultBanner;


        private float startTime;
        private bool m_playing = true;
        
        private EntityManager m_entityManager;
        private Entity m_dataEntity;
        
        private IEnumerator Start()
        {
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            yield return new WaitForSeconds(0.2f);

            m_dataEntity = m_entityManager.CreateEntityQuery(typeof(DataSingleton)).GetSingletonEntity();
            
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
            if (!m_playing) return;
            
            var dataSingleton = m_entityManager.GetComponentData<DataSingleton>(m_dataEntity);

            if (dataSingleton.PlayerDead )
            {
                OnGameEnded(false);
            }
            
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
            m_playing = false;
            
            Time.timeScale = 0f;
            
            m_gameResultsSection.gameObject.SetActive(true);
            
            m_resultText.text = result ? "VICTORY" : "DEFEAT";
            m_resultBanner.color = m_resultText.color = result ? Color.green : Color.red;
            
            
            m_goldsResult.text = PlayerData.CurrentPlayerData.GetGolds().ToString();
            m_killsResults.text = PlayerData.CurrentPlayerData.GetEnemyKills().ToString();
        }
    }
}