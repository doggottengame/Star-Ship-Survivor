using UnityEngine;

public class PlasmaBlockActivate : BlockAtivate
{
    LayerMask enemyLayerMask;
    LayerMask enemyShieldLayer;
    int layerNum, attackLayer;
    [SerializeField]
    Transform enemy, plasmaBodyTrans;
    [SerializeField]
    GameObject plasmaPrefab;
    GameObject plasma;

    bool dockingComplete;

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

            GetComponent<Animator>().SetTrigger("Docking");
        }
    }

    public override void EnemySetRange1(Transform enemyV)
    {
        enemy = enemyV;
    }

    void TargetingEnemy()
    {
        if (!dockingComplete) return;

        if (enemy == null)
        {
            enemy = null;
            plasmaBodyTrans.rotation = Quaternion.Euler(0, 0, 0);

            if (plasma != null)
                Destroy(plasma);

            return;
        }
        else
        {
            if (plasma == null)
            {
                plasma = Instantiate(plasmaPrefab, plasmaBodyTrans);
                plasma.layer = attackLayer;
            }
            plasma.GetComponent<Plasma>().enemyShieldLayer = enemyShieldLayer;
            plasma.GetComponent<Plasma>().Set(enemy, plasmaBodyTrans);
        }

        Vector2 tmp = plasmaBodyTrans.position - enemy.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        plasmaBodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((enemy.position - transform.position).sqrMagnitude > 25)
        {
            Destroy(plasma);
        }
    }

    public void DockingComplete()
    {
        dockingComplete = true;
    }
}
