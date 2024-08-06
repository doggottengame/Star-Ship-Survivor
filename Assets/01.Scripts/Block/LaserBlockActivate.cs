using UnityEngine;

public class LaserBlockActivate : BlockAtivate
{
    LayerMask enemyLayerMask;
    LayerMask enemyShieldLayer;
    int layerNum, attackLayer;
    [SerializeField]
    Transform laserGunBodyTrans;
    Transform enemy;
    [SerializeField]
    GameObject laserPrefab;
    GameObject laser;

    private void Awake()
    {
        blockSet = GetComponent<BlockSet>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetingEnemy();
    }

    public override void Set()
    {
        if (!activated)
        {
            activated = true;
            enemyLayerMask = blockSet.GetEnemyLayerMask();
            enemyShieldLayer = blockSet.GetEnemyShieldLayer();
            layerNum = blockSet.GetLayerNum();
            gameObject.layer = layerNum;
            attackLayer = blockSet.GetAttackLayer();
        }
    }

    public override void EnemySetRange4(Transform enemyV)
    {
        enemy = enemyV;
    }

    void TargetingEnemy()
    {
        if (enemy == null)
        {
            laserGunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);

            if (laser != null)
                Destroy(laser);

            return;
        }
        else
        {
            if (laser == null)
            {
                laser = Instantiate(laserPrefab, laserGunBodyTrans);
                laser.layer = attackLayer;
            }
            laser.GetComponent<Laser>().enemyShieldLayer = enemyShieldLayer;
            laser.GetComponent<Laser>().Set(enemy, laserGunBodyTrans);
        }

        Vector2 tmp = laserGunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        laserGunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
}
