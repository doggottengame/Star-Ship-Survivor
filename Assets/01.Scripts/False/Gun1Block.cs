using System.Collections;
using UnityEngine;

public class Gun1Block : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public int layerNum;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform enemy, gunBodyTrans;
    Animator animator;
    [SerializeField]
    AudioClip fireSound;
    [SerializeField]
    AudioSource fireSoundR, fireSoundL;

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
            gunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Fire", false);
            return;
        }
        Vector2 tmp = gunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        gunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((enemy.position - transform.position).sqrMagnitude < 49)
        {
            animator.SetBool("Fire", true);
        }
        else if ((enemy.position - transform.position).sqrMagnitude < 64)
        {
            animator.SetBool("Fire", false);
        }
    }

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(1);

        while (true)
        {
            Collider2D ps = Physics2D.OverlapCircle(transform.position, 6, enemyLayerMask);
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

    public void FireR()
    {
        GameObject tmp = Instantiate(bulletPrefab, gunBodyTrans);
        tmp.layer = layerNum;
        tmp.transform.localPosition = new Vector3(0.05f, 0.55f, 0);
        fireSoundR.Play();
    }

    public void FireL()
    {
        GameObject tmp = Instantiate(bulletPrefab, gunBodyTrans);
        tmp.layer = layerNum;
        tmp.transform.localPosition = new Vector3(-0.05f, 0.55f, 0);
        fireSoundL.Play();
    }
}
