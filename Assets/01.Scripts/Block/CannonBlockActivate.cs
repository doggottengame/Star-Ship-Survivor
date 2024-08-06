using System.Collections;
using UnityEngine;

public class CannonBlockActivate : BlockAtivate
{
    LayerMask enemyLayerMask;
    int layerNum, attackLayerNum;
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
    Transform cannonBodyTrans;
    Transform enemy;
    bool canFire = true;
    BlockCoreCtrl blockCoreCtrl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            attackLayerNum = blockSet.GetAttackLayer();

            GetComponent<Animator>().SetTrigger("Docking");

            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();
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
            cannonBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        Vector2 tmp = cannonBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        cannonBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (canFire)
        {
            StartCoroutine(FireDelay());
        }
    }

    public void Fire()
    {
        fireSmoke.Play();
        GameObject tmp = Instantiate(bulletPrefab, cannonBodyTrans);
        tmp.layer = attackLayerNum;
        tmp.transform.localPosition = new Vector3(0, 0.72f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass * 5;
        fireSource.Play();
    }

    IEnumerator FireDelay()
    {
        canFire = false;
        animator.SetTrigger("Fire");

        yield return new WaitForSeconds(4);

        canFire = true;
    }
}
