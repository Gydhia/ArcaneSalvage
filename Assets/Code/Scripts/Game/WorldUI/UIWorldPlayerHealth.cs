using System.Collections;
using Assets.Code.Scripts.Game.Player;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldPlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider m_healthSlider;

    private EntityManager m_entityManager;
    private Entity m_playerEntity;
    private IEnumerator Start()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        yield return new WaitForSeconds(0.2f);

        m_playerEntity = m_entityManager.CreateEntityQuery(typeof(InputVariables)).GetSingletonEntity();
        Debug.Log("Player : " + m_playerEntity.ToString());

        var healthData = m_entityManager.GetComponentData<Health>(m_playerEntity);

        m_healthSlider.maxValue = healthData.MaxHealth;
        m_healthSlider.minValue = 0f;

        m_healthSlider.value = healthData.CurrentHealth;
    }

    private void Update()
    {
        float currentPlayerHealth = m_entityManager.GetComponentData<Health>(m_playerEntity).CurrentHealth;
        m_healthSlider.value = currentPlayerHealth;
    }
}
