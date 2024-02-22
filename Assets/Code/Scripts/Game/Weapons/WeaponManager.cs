using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    enum GameModePhase { Phase1, Phase2 }

    [SerializeField] private GameModePhase gameModePhase;
    private Weapon currentWeapon;
    public List<Weapon> Weapons;
    public List<CardInfo> Upgrades = new List<CardInfo>();

    private Transform target;

    private float timer = 0f;

    public static WeaponManager Instance { get; private set; }

    private EntityManager m_entityManager;
    private Entity m_playerEntity;

    public ShootingStraight ShootingStats;

    private void Awake()
    {
        if (currentWeapon == null)
            currentWeapon = Weapons[0];

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        yield return new WaitForSeconds(0.2f);

        m_playerEntity = m_entityManager.CreateEntityQuery(typeof(InputVariables)).GetSingletonEntity();
        CalculateOverrides();
    }

    public void ChooseWeapon(int newWeapon)
    {
        currentWeapon = Weapons[newWeapon];
    }

    public void UpgradeWeapon(CardInfo infos)
    {
        Upgrades.Add(infos);
        CalculateOverrides();
    }

    public void CalculateOverrides()
    {
        var shootingStats = new ShootingStraight();
        var bulletStats = new Bullet();

        shootingStats.FireRate = currentWeapon.firingRate + Upgrades.Where(u => u.upgradeType == UpgradeType.FireRate).Sum(u => u.modifier);
        shootingStats.FireRange = currentWeapon.FireRange + Upgrades.Where(u => u.upgradeType == UpgradeType.MoreRange).Sum(u => u.modifier);
        shootingStats.NumberOfShoot = currentWeapon.NumberOfShoots + Upgrades.Where(u => u.upgradeType == UpgradeType.MoreArrow).Sum(u => u.modifier);
        bulletStats.Damage = currentWeapon.damage;
        shootingStats.ProjectilePrefabEntity = m_entityManager.GetComponentData<ShootingStraight>(m_playerEntity).ProjectilePrefabEntity;

        ShootingStats = shootingStats;
        m_entityManager.SetComponentData<ShootingStraight>(m_playerEntity, ShootingStats);

    }
}
