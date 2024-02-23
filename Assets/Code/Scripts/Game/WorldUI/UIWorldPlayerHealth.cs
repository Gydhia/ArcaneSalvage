using System.Collections;
using Assets.Code.Scripts.Game.Player;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWorldPlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider m_healthSlider;

    private EntityManager m_entityManager;
    private Entity m_playerEntity;
    private void Start()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        StartCoroutine(GetPlayerEntity((() =>
        {
            m_playerEntity = m_entityManager.CreateEntityQuery(typeof(InputVariables)).GetSingletonEntity();

            var healthData = m_entityManager.GetComponentData<Health>(m_playerEntity);

            m_healthSlider.maxValue = healthData.MaxHealth;
            m_healthSlider.minValue = 0f;

            m_healthSlider.value = healthData.CurrentHealth;
        })));
    }
    
    private IEnumerator GetPlayerEntity(UnityAction callback)
    {
        if (m_entityManager.CreateEntityQuery(typeof(InputVariables)).HasSingleton<InputVariables>())
        {
            m_playerEntity = m_entityManager.CreateEntityQuery(typeof(InputVariables)).GetSingletonEntity();
            callback?.Invoke();
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(GetPlayerEntity(callback));
        }
    }
    
    
    private void Update()
    {
        if (m_entityManager.Exists(m_playerEntity))
        {
            float currentPlayerHealth = m_entityManager.GetComponentData<Health>(m_playerEntity).CurrentHealth;
            m_healthSlider.value = currentPlayerHealth;
        }
    }
}
