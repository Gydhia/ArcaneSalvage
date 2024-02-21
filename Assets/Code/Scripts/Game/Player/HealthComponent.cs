using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 100;

    private float m_Health;

    public UnityEvent OnDeath;

    public UnityEvent<float> OnDamaged;

    public UnityEvent<float> OnHealed;

    public float Health
    {
        get => m_Health;
        set
        {
            _CheckForDeath(m_Health, value);
            m_Health = value;
        }
    }

    private void Awake()
    {
        if(OnDeath == null)
            OnDeath = new UnityEvent();
        if(OnDamaged == null)
            OnDamaged = new UnityEvent<float>();
        if (OnHealed == null)
            OnHealed = new UnityEvent<float>();
    }

    private void _CheckForDeath(float iPrevVal, float iCurVal)
    {
        if (iPrevVal > 0 && iCurVal <= 0)
        {
            OnDeath.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Health = MaxHealth;
    }

    public void TakeDamage(float iDamage)
    {
        if(iDamage <= 0 || m_Health <= 0)
            return;

        Health -= (float)Mathf.Min(iDamage, m_Health);
        OnDamaged.Invoke(iDamage);
    }

    public void Heal(float iHeal)
    {
        if (iHeal <= 0 || m_Health >= MaxHealth)
        {
            return;
        }

        Health = (float)Mathf.Min(m_Health + iHeal, MaxHealth);
        OnHealed.Invoke(iHeal);
    }
}