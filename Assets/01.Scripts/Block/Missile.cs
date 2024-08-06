using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform enemy;
    [SerializeField]
    GameObject attackCollider;
    [SerializeField]
    ParticleSystem engineFire;
    [SerializeField]
    AudioSource engineSource;
    [SerializeField]
    DamageMass damageMass;
    [SerializeField]
    BlockAttacked blockAttacked;
    bool fire;

    private void FixedUpdate()
    {
        if (!fire) return;

        if (enemy != null)
        {
            Vector2 tmp = transform.position - enemy.position;
            float rad = Mathf.Atan2(tmp.y, tmp.x);
            float angle = rad * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            rb.AddForce((enemy.position - transform.position).normalized * 2);
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * 2);
        }
    }

    public void Set(Transform enemyV, int attackLayerV, int armoryV)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 0.2f;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.gravityScale = 0;
        engineFire.Play();
        engineSource.Play();

        enemy = enemyV;
        attackCollider.layer = attackLayerV;
        damageMass.dmg += armoryV * 5;
        blockAttacked.maxHp += armoryV * 5;
        blockAttacked.Repair(blockAttacked.maxHp);

        transform.SetParent(null);
        fire = true;
        Destroy(gameObject, 5);
    }
}
