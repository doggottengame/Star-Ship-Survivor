using UnityEngine;

public class ShotgunBlockActivate : BlockAtivate
{
    LayerMask enemyLayerMask;
    int layerNum, attackLayer;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform shotgunBodyTrans;
    Transform enemy;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AudioSource fireSound;
    BlockCoreCtrl blockCoreCtrl;

    bool canFire = true;

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
            layerNum = blockSet.GetLayerNum();
            attackLayer = blockSet.GetAttackLayer();
            gameObject.layer = layerNum;

            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();
        }
    }

    public override void EnemySetRange1(Transform enemyV)
    {
        enemy = enemyV;
    }

    void TargetingEnemy()
    {
        if (enemy == null)
        {
            shotgunBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        Vector2 tmp = shotgunBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        shotgunBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (canFire)
        {
            canFire = false;
            animator.SetTrigger("Fire");
        }
    }

    public void Fire()
    {
        GameObject tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(-0.3f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(-0.18f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(-0.06f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(0.06f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(0.18f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        tmp = Instantiate(bulletPrefab, shotgunBodyTrans);
        tmp.layer = attackLayer;
        tmp.transform.localPosition = new Vector3(0.3f, 0.62f, 0);
        tmp.GetComponent<DamageMass>().dmg += blockCoreCtrl.armoryMass;

        fireSound.Play();
    }

    public void ReadyToFire()
    {
        canFire = true;
    }
}
