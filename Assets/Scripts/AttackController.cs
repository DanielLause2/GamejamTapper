using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public enum EnemyTag { Enemy, Player }

    [Header("Base")]
    public bool AttackActive;
    public EnemyTag Enemy = EnemyTag.Enemy;
    public float Attackspeed = 1;

    [Header("Meele")]
    public bool MeeleActive;
    public int MeeleDamage = 2;
    public float Range = 1;

    [Header("Range")]
    public bool RangeActive;
    public int RangeDamage = 1;
    public Transform ProjectileSpawnPosition;
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 1;
    public int ProjectileAmount = 1;

    private bool meeleAttackCoroutineRunning;
    private bool rangeAttackCoroutineRunning;

    void Awake()
    {
        if (Enemy.ToString() == gameObject.tag)
        {
            Debug.LogError($"Bei dem Objekt \"{gameObject.name}\" ist im AttackController der EnemyTag der eigene Tag.");
        }
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (AttackActive)
        {
            if (MeeleActive)
            {

            }
            else if (RangeActive)
            {
                if (!rangeAttackCoroutineRunning)
                    StartCoroutine(RangeAttack());
            }
        }
    }

    private void MeeleAttack()
    {

    }

    IEnumerator RangeAttack()
    {
        rangeAttackCoroutineRunning = true;
        while (RangeActive)
        {
            List<Transform> enemys = GetClosestEnemy(ProjectileAmount);

            for (int i = 0; i < enemys.Count; i++)
            {
                if (i >= ProjectileAmount)
                    continue;

                GameObject arrow = Instantiate(ProjectilePrefab, ProjectileSpawnPosition.position, Quaternion.identity);
                ProjectileController arrowController = arrow.GetComponent<ProjectileController>();
                arrowController.InitArrow(enemys[i], ProjectileSpeed, RangeDamage);
            }
            yield return new WaitForSecondsRealtime(1 / (Attackspeed <= 0 ? 0.5f : Attackspeed));
        }
        rangeAttackCoroutineRunning = false;
    }

    List<Transform> GetClosestEnemy(int amount)
    {
        // Get enemies as a list:
        List<GameObject> alleGegner = GameObject.FindGameObjectsWithTag(Enemy.ToString()).ToList();

        // Sort the list by distance from ourselves
        alleGegner.Sort(ByDistance);
        alleGegner.Take(amount);
        return alleGegner.Select(x => x.transform).ToList();
    }

    int ByDistance(GameObject a, GameObject b)
    {
        var dstToA = Vector3.Distance(transform.position, a.transform.position);
        var dstToB = Vector3.Distance(transform.position, b.transform.position);
        return dstToA.CompareTo(dstToB);
    }
}
