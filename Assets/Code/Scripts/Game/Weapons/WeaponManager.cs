using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    enum GameModePhase { Phase1, Phase2 }

    [SerializeField] private GameModePhase gameModePhase;
    private Weapon currentWeapon;
    public GameObject bow;
    public GameObject wand;

    private Transform target;

    private float timer = 0f;

    public void ChooseWeapon(int newWeapon)
    {
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        switch(newWeapon)
        {
            case 1 :
                currentWeapon = bow.GetComponent<Weapon>();
                break;
            case 2:
                currentWeapon = wand.GetComponent<Weapon>();
                break;
            default:
                currentWeapon = bow.GetComponent<Weapon>();
                break;
        }
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
