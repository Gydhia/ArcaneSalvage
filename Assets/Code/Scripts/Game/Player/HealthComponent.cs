using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private sbyte MaxHealth = 100;

    private sbyte m_Health;

    public UnityEvent OnDeath;

    public UnityEvent<int> OnDamaged;

    public UnityEvent<int> OnHealed;

    public sbyte Health
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
            OnDamaged = new UnityEvent<int>();
        if (OnHealed == null)
            OnHealed = new UnityEvent<int>();
    }

    private void _CheckForDeath(sbyte iPrevVal, sbyte iCurVal)
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

    public void TakeDamage(sbyte iDamage)
    {
        if(iDamage <= 0 || m_Health <= 0)
            return;

        Health -= (sbyte)Mathf.Min(iDamage, m_Health);
        OnDamaged.Invoke(iDamage);
    }

    public void Heal(sbyte iHeal)
    {
        if (iHeal <= 0 || m_Health >= MaxHealth)
        {
            return;
        }

        Health = (sbyte)Mathf.Min(m_Health + iHeal, MaxHealth);
        OnHealed.Invoke(iHeal);
    }
}