using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum MenuSection
{
    None = 0,

    Play = 1,
    Equipment = 2,
    Settings = 3
}

namespace ArcanaSalvage.UI
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        [SerializeField] public List<UIMenu> Menus;

        private UIMenu m_currentMenu;

        protected override void Awake()
        {
            base.Awake();
            foreach (var menu in Menus)
            {
                menu.gameObject.SetActive(false);
            }
        }

        public void ChangeMenu(MenuSection section)
        {
            if (m_currentMenu != null)
            {
                if (m_currentMenu.MenuSection == section)
                    return;

                m_currentMenu.OnMenuClose();
                m_currentMenu.gameObject.SetActive(false);
            }

            m_currentMenu = Menus.Single(m => m.MenuSection == section);

            m_currentMenu.gameObject.SetActive(true);
            m_currentMenu.OnMenuOpen();
        }

        public UIMenu GetMenu(MenuSection section)
        {
            return Menus.Single(m => m.MenuSection == section);
        }

        public void StartGame()
        {
            SceneController.Instance.LoadScene("Game");
        }
    }
}