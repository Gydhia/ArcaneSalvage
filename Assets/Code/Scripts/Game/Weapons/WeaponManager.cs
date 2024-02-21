using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    enum GameModePhase { Phase1, Phase2 }

    [SerializeField] private GameModePhase gameModePhase;
    public Weapon currentWeapon;
    public GameObject bow;
    public GameObject wand;

    public Transform target;

    private float timer = 0f;

    public void ChangeWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = Instantiate(newWeapon, transform);
    }

    public void UpgradeWeapon(float extraDamage, float extraFireRate)
    {
        if (currentWeapon != null)
        {
            currentWeapon.damage += extraDamage;
            currentWeapon.firingRate += extraFireRate;
        }
    }

    void Update()
    {
        if (currentWeapon != null)
        {
            timer += Time.deltaTime;
            if (timer >= currentWeapon.firingRate)
            {
                if (gameModePhase == GameModePhase.Phase1)
                {
                    FindClosestEnemy();
                        currentWeapon.Fire(target);
                }
                else
                {
                    // code phase 2
                }
                timer = 0;
            }
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = currentWeapon.transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                target = enemy.transform;
                minDistance = distance;
            }
        }
    }
}
