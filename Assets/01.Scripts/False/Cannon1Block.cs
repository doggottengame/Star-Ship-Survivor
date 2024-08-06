using System.Collections;
using UnityEngine;

public class Cannon1Block : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public int layerNum;
    Animator animator;
    [SerializeField]
    ParticleSystem fireSmoke;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    AudioClip fireSound;
    [SerializeField]
    AudioSource fireSource;
    [SerializeField]
    Transform enemy, cannonBodyTrans;
    bool canFire = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(EnemySearch());
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
            cannonBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Fire", false);
            return;
        }
        Vector2 tmp = cannonBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        cannonBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((enemy.position - transform.position).sqrMagnitude < 100)
        {
            if (canFire)
            {
                StartCoroutine(FireDelay());
                animator.SetTrigger("Fire");
            }
        }
    }

    public void Fire()
    {
        fireSmoke.Play();
        GameObject tmp = Instantiate(bulletPrefab, cannonBodyTrans);
        tmp.layer = layerNum;
        tmp.transform.localPosition = new Vector3(0, 0.72f, 0);
        fireSource.Play();
    }

    IEnumerator FireDelay()
    {
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
