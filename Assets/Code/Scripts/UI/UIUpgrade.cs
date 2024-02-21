using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcanaSalvage.UI
{
    public class UIUpgrade : MonoBehaviour
    {
        [SerializeField] private Image m_upradeIcon;
        
        [SerializeField] private TextMeshProUGUI m_upradeName;
        [SerializeField] private TextMeshProUGUI m_upradeDescription;

        [SerializeField] private Button m_upgradeButton;

        public Button UpgradeButton => m_upgradeButton;

        public void Init(/* TODO : Add scriptable as parameter and set values */)
        {
            
        }
    }
}