using System;
using System.Collections;
using System.Collections.Generic;
using ArcanaSalvage;
using ArcanaSalvage.UI;
using Code.Scripts.Game.Authoring;
using Code.Scripts.Game.Player;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class CallEndPhaseTwo : MonoBehaviour
{
    
    private EntityManager m_entityManager;
    private Entity m_invEntity;
    private void Start()
        {
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            StartCoroutine(GetPlayerEntity());
            
        }
        
        private IEnumerator GetPlayerEntity()
        {
            if (m_entityManager.CreateEntityQuery(typeof(Inventory)).HasSingleton<Inventory>())
            {
                m_invEntity = m_entityManager.CreateEntityQuery(typeof(Inventory)).GetSingletonEntity();
                
            }
            else
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(GetPlayerEntity());
            }
        }
        
        
        public void Update()
        {
            if (m_entityManager.Exists(m_invEntity))
            {
                var invSingleton = m_entityManager.GetComponentData<Inventory>(m_invEntity);
                
                if (invSingleton.PlayerDead)
                {
                    GameUIManager.Instance.OnGameEnded(false);
                }

                if (invSingleton.KillsCounter == 6)
                {
                    GameUIManager.Instance.OnGameEnded(true);
                }
            }
        }
}
