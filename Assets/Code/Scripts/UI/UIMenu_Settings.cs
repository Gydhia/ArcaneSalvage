using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaSalvage.UI
{
    public class UIMenu_Settings : UIMenu
    {
        [SerializeField] private Slider m_soundSlider;
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Toggle m_vibration;

        [SerializeField] private TextMeshProUGUI m_soundValue;
        [SerializeField] private TextMeshProUGUI m_musicValue;
        
        private void Awake()
        {
            m_soundSlider.onValueChanged.AddListener((val) => m_soundValue.text = ((int)val).ToString());
            m_musicSlider.onValueChanged.AddListener((val) => m_musicValue.text = ((int)val).ToString());
        }
    }
}