using System.Collections;
using UnityEngine;

public class RailGun1Block : MonoBehaviour
{
    [SerializeField]
    ParticleSystem electricL, electricR;
    public LayerMask enemyLayerMask;
    public int layerNum;
    [SerializeField]
    GameObject bulletPrefab;
    GameObject bullet;
    [SerializeField]
    AudioSource fireSource;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform enemy, railgunBodyTrans;
    bool canFire = true;

    private void Awake()
    {
        StartCoroutine(EnemySearch());
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetingEnemy();
    }

    void TargetingEnemy()
    {
        if (enemy == null)
        {
            railgunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        Vector2 tmp = railgunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        railgunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((enemy.position - transform.position).sqrMagnitude < 100)
        {
            if (canFire)
            {
                StartCoroutine(FireDelay());
            }
        }
    }

    public void Fire()
    {
        bullet.GetComponent<RailGunBullet>().Fire();
        bullet = null;
    }

    IEnumerator FireDelay()
    {
        bullet = Instantiate(bulletPrefab, railgunBodyTrans);
        bullet.layer = layerNum;
        bullet.transform.localPosition = new Vector3(0, -0.31f, 0);
        electricL.Play();
        electricR.Play();

        animator.SetTrigger("Fire");
        fireSource.Play();

        canFire = false;

        yield return new WaitForSeconds(5);

        canFire = true;
    }

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(1);

        while (true)
        {
            Collider2D ps = Physics2D.OverlapCircle(transform.position, 10, enemyLayerMask);
            if (ps == null)
            {
                enemy = null;
            }
            else
            {
                if (enemy == null || (ps.transform.position - transform.position).sqrMagnitude < (enemy.position - transform.position).sqrMagnitude)
                {
                    enemy = ps.transform;
                }
            }

            yield return seconds;
        }
    }
}
