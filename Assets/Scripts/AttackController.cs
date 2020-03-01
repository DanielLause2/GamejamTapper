using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public enum EnemyTag { Enemy, Player }
    public enum RangeAnimatorStates { Idle, AttackUP, AttackDOWN, AttackLEFT, AttackRIGHT }

    [Header("Base")]
    public bool AttackActive;
    public EnemyTag Enemy = EnemyTag.Enemy;
    public float Attackspeed = 1;

    [Header("Meele")]
    public bool MeeleActive;
    public Animator MeeleAnimator;
    public int MeeleDamage = 2;
    public float Range = 1;

    [Header("Range")]
    public bool RangeActive;
    public Animator RangeAnimator;
    public int RangeDamage = 1;
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 1;
    public int ProjectileAmount = 1;
    public Transform projectileSpawnPositionUp;
    public Transform projectileSpawnPositionDown;
    public Transform projectileSpawnPositionLeft;
    public Transform projectileSpawnPositionRight;


    private bool meeleAttackCoroutineRunning;
    private bool rangeAttackCoroutineRunning;
    private Transform projectileSpawnPosition;

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

                if (ProjectileAmount > 0)
                {
                    Vector3 enemyPosition = enemys[i].position;
                    float horizontalDistance = 0;
                    float verticalDistance = 0;
                    bool upAttack = true;
                    bool rightAttack = true;

                    if (enemyPosition.x > transform.position.x)
                    {
                        rightAttack = true;
                        horizontalDistance = enemyPosition.x - transform.position.x;
                    }
                    else if (enemyPosition.x < transform.position.x)
                    {
                        rightAttack = false;
                        horizontalDistance = (transform.position.x - enemyPosition.x);
                    }

                    if (enemyPosition.y > transform.position.y)
                    {
                        upAttack = true;
                        verticalDistance = enemyPosition.y - transform.position.y;
                    }
                    else if (enemyPosition.y < transform.position.y)
                    {
                        upAttack = false;
                        verticalDistance = (transform.position.y - enemyPosition.y);
                    }

                    if (horizontalDistance > verticalDistance)
                    {
                        if (rightAttack)
                            SetRangeAnimator(RangeAnimatorStates.AttackRIGHT);
                        else
                            SetRangeAnimator(RangeAnimatorStates.AttackLEFT);
                    }
                    else
                    {
                        if (upAttack)
                            SetRangeAnimator(RangeAnimatorStates.AttackUP);
                        else
                            SetRangeAnimator(RangeAnimatorStates.AttackDOWN);
                    }

                }

                GameObject arrow = Instantiate(ProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
                ProjectileController arrowController = arrow.GetComponent<ProjectileController>();
                arrowController.InitArrow(enemys[i], ProjectileSpeed, RangeDamage);

            }
            yield return new WaitForSecondsRealtime(1 / (Attackspeed <= 0 ? 0.5f : Attackspeed));
        }
        SetRangeAnimator(RangeAnimatorStates.Idle);
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

    private void SetRangeAnimator(RangeAnimatorStates state)
    {
        switch (state)
        {
            case RangeAnimatorStates.Idle:
                RangeAnimator.SetBool(RangeAnimatorStates.Idle.ToString(), true);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackUP.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackDOWN.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackLEFT.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackRIGHT.ToString(), false);
                break;
            case RangeAnimatorStates.AttackUP:
                projectileSpawnPosition = projectileSpawnPositionUp;
                RangeAnimator.SetBool(RangeAnimatorStates.Idle.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackUP.ToString(), true);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackDOWN.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackLEFT.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackRIGHT.ToString(), false);
                break;
            case RangeAnimatorStates.AttackDOWN:
                projectileSpawnPosition = projectileSpawnPositionDown;
                RangeAnimator.SetBool(RangeAnimatorStates.Idle.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackUP.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackDOWN.ToString(), true);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackLEFT.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackRIGHT.ToString(), false);
                break;
            case RangeAnimatorStates.AttackLEFT:
                projectileSpawnPosition = projectileSpawnPositionLeft;
                RangeAnimator.SetBool(RangeAnimatorStates.Idle.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackUP.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackDOWN.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackLEFT.ToString(), true);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackRIGHT.ToString(), false);
                break;
            case RangeAnimatorStates.AttackRIGHT:
                projectileSpawnPosition = projectileSpawnPositionRight;
                RangeAnimator.SetBool(RangeAnimatorStates.Idle.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackUP.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackDOWN.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackLEFT.ToString(), false);
                RangeAnimator.SetBool(RangeAnimatorStates.AttackRIGHT.ToString(), true);
                break;
            default:
                break;
        }
    }
}
