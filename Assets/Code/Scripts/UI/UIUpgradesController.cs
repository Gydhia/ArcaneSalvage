using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaSalvage.UI
{
    public class UIUpgradesController : MonoBehaviour
    {
        [SerializeField]
        private List<UIUpgrade> m_upgrades;

        private void Awake()
        {
            for (int i = 0; i < m_upgrades.Count; i++)
            {
                var upgrade = m_upgrades[i];
                upgrade.UpgradeButton.onClick.AddListener(
                    () => OnClickUpgrade(upgrade)
                    );
            }
        }

        public void RefreshUpgrade()
        {
            gameObject.SetActive(true);
            
            for (int i = 0; i < m_upgrades.Count; i++)
            {
                // TODO : Refresh the upgrades
                // m_upgrades[i].Init();
            }
        }

        private void OnClickUpgrade(UIUpgrade selectedUpgrade)
        {
            // TODO CHECK : not getting the right selected upgrade ? This might comes from the Awake with AddListener capturing the wrong variable (ask Killian)
            
            
        }
    }
}