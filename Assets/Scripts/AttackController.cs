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
    public float Damage = 1;

    [Header("Meele")]
    public bool MeeleActive;
    public float Range = 1;

    [Header("Range")]
    public bool RangeActive;
    public Transform ArrowSpawnPosition;
    public GameObject ArrowPrefab;
    public float ArrowSpeed = 1;
    public int ArrowAmount = 1;

    [HideInInspector]
    public bool CharakterIsMoving;

    private bool meeleAttackCoroutineRunning;
    private bool rangeAttackCoroutineRunning;

    void Awake()
    {
        if (Enemy.ToString() == gameObject.tag)
        {
            Debug.LogError($"Bei dem Objekt \"{gameObject.name}\" ist im AttackController der EnemyTag der eigene Tag.");
        }

        Debug.Log(1 / Attackspeed / 100);
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        RangeActive = !CharakterIsMoving;
        MeeleActive = CharakterIsMoving;

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
            List<Transform> enemys = GetClosestEnemy(ArrowAmount);

            for (int i = 0; i < enemys.Count; i++)
            {
                if (i >= ArrowAmount)
                    continue;

                GameObject arrow = Instantiate(ArrowPrefab, ArrowSpawnPosition.position, Quaternion.identity);
                ArrowController arrowController = arrow.GetComponent<ArrowController>();
                arrowController.SetEnemy(enemys[i]);
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
