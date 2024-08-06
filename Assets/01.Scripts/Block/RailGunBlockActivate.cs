using System.Collections;
using UnityEngine;

public class RailGunBlockActivate : BlockAtivate
{
    [SerializeField]
    ParticleSystem electricL, electricR;
    LayerMask enemyLayerMask;
    int layerNum, attackLayer;
    [SerializeField]
    GameObject bulletPrefab;
    GameObject bullet;
    [SerializeField]
    AudioSource fireSource;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform railgunBodyTrans;
    Transform enemy;
    bool canFire = true;
    BlockCoreCtrl blockCoreCtrl;

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
            layerNum = blockSet.GetLayerNum();
            attackLayer = blockSet.GetAttackLayer();

            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();
        }
    }

    public override void EnemySetRange5(Transform enemyV)
    {
        enemy = enemyV;
    }

    void TargetingEnemy()
    {
        if (enemy == null)
        {
            railgunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Fire", false);
            return;
        }
        Vector2 tmp = railgunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        railgunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (canFire)
        {
            StartCoroutine(FireDelay());
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
        bullet.layer = attackLayer;
        bullet.transform.localPosition = new Vector3(0, -0.31f, 0);
        bullet.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass * 8;
        bullet.GetComponent<DamageMass>().dmg += blockCoreCtrl.energyGeneratorMass * 8;
        electricL.Play();
        electricR.Play();

        animator.SetTrigger("Fire");
        fireSource.Play();

        canFire = false;

        yield return new WaitForSeconds(4);

        canFire = true;
    }
}
