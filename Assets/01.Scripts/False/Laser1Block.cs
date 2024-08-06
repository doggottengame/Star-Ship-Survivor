using System.Collections;
using UnityEngine;

public class Laser1Block : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public LayerMask enemyShieldLayer;
    public int layerNum;
    [SerializeField]
    Transform enemy, laserGunBodyTrans;
    [SerializeField]
    GameObject laserPrefab;
    GameObject laser;

    private void Start()
    {
        StartCoroutine(EnemySearch());
    }

    // Update is called once per frame
    void Update()
    {
        TargetingEnemy();
    }

    void TargetingEnemy()
    {
        if (enemy == null || laser == null)
        {
            laserGunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        Vector2 tmp = laserGunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        laserGunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((enemy.position - transform.position).sqrMagnitude > 81)
        {
            Destroy(laser);
        }
    }

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        while (true)
        {
            Collider2D ps = Physics2D.OverlapCircle(transform.position, 9, enemyLayerMask);
            if (ps == null)
            {
                enemy = null;
            }
            else
            {
                if ((enemy == null || (enemy != ps.transform && (ps.transform.position - transform.position).sqrMagnitude < (enemy.position - transform.position).sqrMagnitude)) && ps.gameObject.activeSelf)
                {
                    enemy = ps.transform;
                    if (laser == null)
                    {
                        laser = Instantiate(laserPrefab, laserGunBodyTrans);
                        laser.layer = layerNum;
                    }
                    laser.GetComponent<Laser>().enemyShieldLayer = enemyShieldLayer;
                    laser.GetComponent<Laser>().Set(enemy, laserGunBodyTrans);
                }
            }

            yield return seconds;
        }
    }
}
