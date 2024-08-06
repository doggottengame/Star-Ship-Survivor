using System.Collections;
using UnityEngine;

public class MissileBlockActivate : BlockAtivate
{
    BlockCoreCtrl blockCoreCtrl;
    [SerializeField]
    GameObject missilePrefab;
    GameObject missile1, missile2;
    [SerializeField]
    Transform missileBodyTrans;
    Transform enemy;
    AudioSource audioSource;
    LayerMask enemyLayerMask;
    int droneLayer, playerAttackLayer;

    bool canFire = true;

    private void Awake()
    {
        blockSet = GetComponent<BlockSet>();
    }

    public void Update()
    {
        if (enemy == null)
        {
            missileBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        else
        {
            Vector2 tmp = missileBodyTrans.position - enemy.position;
            float rad = Mathf.Atan2(tmp.y, tmp.x);
            float angle = rad * Mathf.Rad2Deg;
            missileBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

            if (canFire)
            {
                StartCoroutine(FireDelay());
            }
        }
    }

    public override void Set()
    {
        if (!activated)
        {
            activated = true;
            enemyLayerMask = blockSet.GetEnemyLayerMask();
            droneLayer = blockSet.GetDroneLayer();
            playerAttackLayer = blockSet.GetAttackLayer();
            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();

            audioSource = GetComponent<AudioSource>();

            missile1 = Instantiate(missilePrefab, missileBodyTrans);
            missile1.transform.localPosition = new Vector3(-0.37f, 0, 0);

            missile2 = Instantiate(missilePrefab, missileBodyTrans);
            missile2.transform.localPosition = new Vector3(0.37f, 0, 0);
        }
    }

    public override void EnemySetRange5(Transform enemyV)
    {
        enemy = enemyV;
    }

    IEnumerator FireDelay()
    {
        canFire = false;

        audioSource.Play();

        yield return new WaitForSeconds(0.5f);

        if (missile1 != null)
        {
            missile1.layer = droneLayer;
            missile1.GetComponent<Missile>().Set(enemy, playerAttackLayer, blockCoreCtrl.armoryMass);
        }
        if (missile2 != null)
        {
            missile2.layer = droneLayer;
            missile2.GetComponent<Missile>().Set(enemy, playerAttackLayer, blockCoreCtrl.armoryMass);
        }

        yield return new WaitForSeconds(4);

        missile1 = Instantiate(missilePrefab, missileBodyTrans);
        missile1.transform.localPosition = new Vector3(-0.37f, 0, 0);

        missile2 = Instantiate(missilePrefab, missileBodyTrans);
        missile2.transform.localPosition = new Vector3(0.37f, 0, 0);

        canFire = true;
    }
}
