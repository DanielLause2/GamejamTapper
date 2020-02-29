using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Transform ProjectileSprite;
    public float Speed = 1;
    public int Damage = 1;

    private Vector3 normalizeDirection;

    public void InitArrow(Transform vEnemy, float vSpeed, int vDamage)
    {
        Speed = vSpeed;
        Damage = vDamage;
        normalizeDirection = (vEnemy.position - transform.position).normalized;
    }

    void Start()
    {
        StartCoroutine(Destroy(20));
    }

    void Update()
    {

        if (normalizeDirection != null)
        {
            transform.position += normalizeDirection * Speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

    }

    IEnumerator Destroy(float destroyTime)
    {
        yield return new WaitForSecondsRealtime(destroyTime);
        Destroy(this.gameObject);
    }
}
