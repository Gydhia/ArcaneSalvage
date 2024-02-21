using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ArcanaSalvage.UI
{
    [RequireComponent(typeof(Toggle))]
    public class UIMenuToggle : MonoBehaviour
    {
        [SerializeField] private Toggle m_selfToggle;
        [SerializeField] private MenuSection m_redirectMenu;

        private void Awake()
        {
            m_selfToggle.onValueChanged.AddListener((state) =>
            {
                if (state)
                {
                    MainMenuManager.Instance.ChangeMenu(m_redirectMenu);
                }
            });
        }
    }
}