using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using InputManager = Code.Scripts.Game.Player.InputManager;

namespace ArcanaSalvage.UI
{
    public enum CornerDirection
    {
        BotLeft,
        TopLeft,
        TopRight,
        BotRight
    }
    
    public class UIJoystick : MonoBehaviour
    {
        [SerializeField] private List<Image> m_focusDatas;
        
        [SerializeField] private GameObject m_visuals;
        [SerializeField] private RectTransform m_pad;
        
        private Dictionary<CornerDirection, Image> m_focusImages;
        private Image m_currentFocus;
        private RectTransform m_selfTransform;

        private void OnValidate()
        {
            m_selfTransform = GetComponent<RectTransform>();
            
            if (m_focusDatas == null || m_focusDatas.Count < 4) return;
            
            m_focusImages = new Dictionary<CornerDirection, Image>();

            m_focusImages.Add(CornerDirection.BotLeft, m_focusDatas[0]);
            m_focusImages.Add(CornerDirection.TopLeft, m_focusDatas[1]);
            m_focusImages.Add(CornerDirection.TopRight, m_focusDatas[2]);
            m_focusImages.Add(CornerDirection.BotRight, m_focusDatas[3]);
        }

        private void Start()
        {
            m_visuals.SetActive(false);

            InputManager.Instance.OnTouchStart += OnTouchStart;
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnTouchStart -= OnTouchStart;
        }

        private void OnTouchStart(Vector2 pointer)
        {
            m_visuals.SetActive(true);
            
            m_pad.anchorMin = m_pad.anchorMax = (pointer + Vector2.one) / 2f;

            m_selfTransform.position = pointer;
        }

        private void Update()
        {
            if (InputManager.Instance.IsTouching)
            {
                var newFocus= GetCornerDirection(InputManager.Instance.MoveDirection);
                if (newFocus != m_currentFocus)
                {
                    if (m_currentFocus != null)
                    {
                        m_currentFocus.DOFade(0f, 0.15f);
                    }
                    m_currentFocus = newFocus;
                    m_currentFocus.DOFade(1f, 0.15f);
                }
                
                m_pad.anchorMin = m_pad.anchorMax = (InputManager.Instance.MoveDirection + Vector2.one) / 2f;
            }
            else
            {
                m_visuals.SetActive(false);
            }
        }
        
        public Image GetCornerDirection(Vector2 moveDirection)
        {
            if (moveDirection.x < 0)
            {
                if (moveDirection.y < 0)
                    return  m_focusImages[CornerDirection.BotLeft];
                
                return m_focusImages[CornerDirection.TopLeft];
            }
            
            if (moveDirection.y < 0)
                return m_focusImages[CornerDirection.BotRight];
        
            return m_focusImages[CornerDirection.TopRight];
        }
    }
}
