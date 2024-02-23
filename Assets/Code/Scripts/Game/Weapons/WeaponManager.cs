using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArcanaSalvage;
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
    private ShootingCardinal m_shootingCardinal = new ShootingCardinal();

    public ShootingStraight ShootingStats;
    private bool m_firstTime = true;

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

        // Set the player max HP according to equipment
        if (m_entityManager.Exists(m_playerEntity))
        {
            var playerHealth = m_entityManager.GetComponentData<Health>(m_playerEntity);

            playerHealth.MaxHealth += PlayerData.CurrentPlayerData.GetHealthOverride();
            
            m_entityManager.SetComponentData<Health>(m_playerEntity, playerHealth);
        }
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

        shootingStats.BulletMoveSpeed = currentWeapon.speed + Upgrades.Where(u => u.upgradeType == UpgradeType.MoreSpeed).Sum(u => u.modifier);
        shootingStats.OriginalFireRate = currentWeapon.firingRate + Upgrades.Where(u => u.upgradeType == UpgradeType.FireRate).Sum(u => u.modifier);
        shootingStats.FireRate = shootingStats.OriginalFireRate;
        shootingStats.FireRange = currentWeapon.FireRange + Upgrades.Where(u => u.upgradeType == UpgradeType.MoreRange).Sum(u => u.modifier);
        shootingStats.NumberOfShoot = currentWeapon.NumberOfShoots + Upgrades.Where(u => u.upgradeType == UpgradeType.MoreArrow).Sum(u => u.modifier);
        shootingStats.BulletDamage = currentWeapon.damage + Upgrades.Where(u => u.upgradeType == UpgradeType.Damage).Sum(u => u.modifier);
        shootingStats.OwnerType = OwnerType.Player;
        shootingStats.ProjectilePrefabEntity = m_entityManager.GetComponentData<ShootingStraight>(m_playerEntity).ProjectilePrefabEntity;

        ShootingStats = shootingStats;
        m_entityManager.SetComponentData<ShootingStraight>(m_playerEntity, ShootingStats);

        if (Upgrades.Exists(x => x.upgradeType == UpgradeType.Angle))
        {
            m_shootingCardinal.BulletMoveSpeed = shootingStats.BulletMoveSpeed;
            m_shootingCardinal.OriginalFireRate = shootingStats.OriginalFireRate;
            m_shootingCardinal.FireRate = m_shootingCardinal.OriginalFireRate;
            m_shootingCardinal.BulletMoveSpeed = shootingStats.BulletDamage;
            m_shootingCardinal.BulletDamage = shootingStats.BulletDamage;
            m_shootingCardinal.ProjectilePrefabEntity = shootingStats.ProjectilePrefabEntity;
            m_shootingCardinal.OwnerType = shootingStats.OwnerType;

            if (m_firstTime)
            {
                int number = Random.Range(0, 11);
                if (number < 5)
                {
                    m_shootingCardinal.ShootingDirection = ShootingDirection.CARDINAL;
                }
                else
                {
                    if (number < 10)
                    {
                        m_shootingCardinal.ShootingDirection = ShootingDirection.INTERCARDINAL;
                    }
                    else
                    {
                        m_shootingCardinal.ShootingDirection = ShootingDirection.BOTH;
                    }
                }
                m_firstTime = false;
            }

            m_entityManager.AddComponentObject(m_playerEntity, m_shootingCardinal);
        }

        

    }
}
