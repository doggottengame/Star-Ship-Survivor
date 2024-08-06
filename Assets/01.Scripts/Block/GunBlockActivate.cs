using UnityEngine;

public class GunBlockActivate : BlockAtivate
{
    LayerMask enemyLayerMask;
    int layerNum, attackLayer;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform gunBodyTrans;
    Transform enemy;
    Animator animator;
    [SerializeField]
    AudioSource fireSound;
    BlockCoreCtrl blockCoreCtrl;

    bool fire;

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
            attackLayer = blockSet.GetAttackLayer();
            gameObject.layer = layerNum;

            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();
        }
    }

    public override void EnemySetRange2(Transform enemyV)
    {
        enemy = enemyV;
    }

    void TargetingEnemy()
    {
        if (enemy == null)
        {
            gunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("Fire", false);
            fire = false;
            return;
        }
        Vector2 tmp = gunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        gunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        animator.SetBool("Fire", true);
        fire = true;
    }

    public void FireR()
    {
        if (!fire) return;
        GameObject tmp = Instantiate(bulletPrefab, gunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(0.05f, 0.55f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;
        fireSound.Play();
    }

    public void FireL()
    {
        if (!fire) return;
        GameObject tmp = Instantiate(bulletPrefab, gunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(-0.05f, 0.55f, 0);
        fireSound.Play();
    }
}
