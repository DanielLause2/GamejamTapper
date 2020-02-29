using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float Speed;
    public float Damage;

    private Vector3 target;

    public void SetEnemy(Transform vEnemy)
    {
        target = vEnemy.position;
    }

    void Start()
    {
        StartCoroutine(Destroy(5));
    }

    void Update()
    {

        if (target != null)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);

            var lookPos = target - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,  100);
        }
    }

    IEnumerator Destroy(float destroyTime)
    {
        yield return new WaitForSecondsRealtime(destroyTime);
        Destroy(this.gameObject);
    }
}
