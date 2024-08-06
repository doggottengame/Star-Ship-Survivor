using System.Collections;
using UnityEngine;

public class EnemyCtrl : BlockCoreCtrl
{
    [SerializeField]
    Transform destination;
    Transform player;

    Vector3 dir;
    [SerializeField]
    LayerMask blockLayer;
    [SerializeField]
    bool isBoss;

    private void Awake()
    {
        player = GameManager.instance.player.transform;

        rb = GetComponent<Rigidbody2D>();
        blockMass = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        BlockSet[] blockSets = GetComponentsInChildren<BlockSet>();
        foreach (BlockSet sets in blockSets)
        {
            if (sets != null)
            {
                sets.Set(layerNum, droneLayer, playerAttackLayer, shieldLayer, enemyLayerMask, enemyShieldLayerMask);
            }
        }

        BlockAtivate[] blockAtivates = GetComponentsInChildren<BlockAtivate>();
        foreach (BlockAtivate block in blockAtivates)
        {
            if (block != null)
            {
                block.Set();
            }
        }

        destination = player;
        StartCoroutine(EnemySearch());
    }

    private void FixedUpdate()
    {
        SpeedCtrl();
    }

    void SpeedCtrl()
    {
        if (destination == null) return;
        dir = (destination.transform.position - transform.position).normalized;
        if (dir.x != 0 || dir.y != 0)
        {
            if (!onFire)
            {
                engines = blockTrans.GetComponentsInChildren<Engine>();
                if (engines.Length > 0 || mainEngine != null)
                {
                    audioSource.Play();
                }
                foreach (Engine engine in engines)
                {
                    engine.FireEngine();
                }
            }

            onFire = true;
        }
        else
        {
            if (onFire)
            {
                engines = blockTrans.GetComponentsInChildren<Engine>();

                audioSource.Stop();

                foreach (Engine engine in engines)
                {
                    engine.StopEngine();
                }
            }

            onFire = false;
        }

        vel = rb.velocity;
        if (Mathf.Abs(vel.x) < speed / 4 || vel.x * dir.x < 0)
        {
            rb.AddForce(Vector3.right * dir.x * xSpeed * speed / 4);
        }
        if (Mathf.Abs(vel.y) < speed / 4 || vel.y * dir.y < 0)
        {
            rb.AddForce(Vector3.up * dir.y * ySpeed * speed / 4);
        }
    }

    public override void BlockCombine(string blockName, GameObject block)//, GameObject conObj)
    {
        if (destination != player)
        {
            destination = null;
        }
        block.transform.SetParent(blockTrans);
        block.transform.localPosition = new Vector3(Mathf.RoundToInt(block.transform.localPosition.x), Mathf.RoundToInt(block.transform.localPosition.y), block.transform.localPosition.z);
        block.layer = gameObject.layer;
        BlockMassChanged();

        if (block.GetComponent<BlockAtivate>() != null)
        {
            block.GetComponent<BlockAtivate>().blockSet.Set(layerNum, droneLayer, playerAttackLayer, shieldLayer, enemyLayerMask, enemyShieldLayerMask);
            block.GetComponent<BlockAtivate>().Set();
        }
    }

    public override void BlockMassChanged()
    {
        blockMass = blockTrans.childCount;
        rb.mass = 0.5f + (blockMass - 1) * 0.3f;
        speed =
            10 * (mainEngine == null ? 0 : 1) +
            5 * (engines == null ? 0 : engines.Length - 1);
        xSpeed =
            2 * (mainEngine == null ? 0 : 1) +
            1 * (engines == null ? 0 : engines.Length - 1);
        ySpeed =
            2 * (mainEngine == null ? 0 : 1) +
            1 * (engines == null ? 0 : engines.Length - 1);

        if (engines.Length == 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.1f);
        BlockAtivate[] blockAtivates;
        Transform enemy1 = null, enemy2 = null, enemy3 = null, enemy4 = null, enemy5 = null;
        Collider2D ps1, ps2, ps3, ps4, ps5;
        float rangeInc;

        while (true)
        {
            rangeInc = 1 + 0.3f * radarMass;
            blockAtivates = GetComponentsInChildren<BlockAtivate>();

            ps1 = Physics2D.OverlapCircle(transform.position, 4 * rangeInc, enemyLayerMask);
            if (ps1 == null)
            {
                enemy1 = null;
            }
            else
            {
                if (enemy1 == null)
                {
                    enemy1 = ps1.transform;
                }
                else
                {
                    if ((ps1.transform.position - transform.position).sqrMagnitude < (enemy1.position - transform.position).sqrMagnitude)
                    {
                        enemy1 = ps1.transform;
                    }
                }
            }

            foreach (BlockAtivate activate in blockAtivates)
            {
                activate.EnemySetRange1(enemy1);
            }

            ps2 = Physics2D.OverlapCircle(transform.position, 5 * rangeInc, enemyLayerMask);
            if (ps2 == null)
            {
                enemy2 = null;
            }
            else
            {
                if (enemy2 == null)
                {
                    enemy2 = ps2.transform;
                }
                else
                {
                    if ((ps2.transform.position - transform.position).sqrMagnitude < (enemy2.position - transform.position).sqrMagnitude)
                    {
                        enemy2 = ps2.transform;
                    }
                }
            }

            foreach (BlockAtivate activate in blockAtivates)
            {
                activate.EnemySetRange2(enemy2);
            }

            ps3 = Physics2D.OverlapCircle(transform.position, 6 * rangeInc, enemyLayerMask);
            if (ps3 == null)
            {
                enemy3 = null;
            }
            else
            {
                if (enemy3 == null)
                {
                    enemy3 = ps3.transform;
                }
                else
                {
                    if ((ps3.transform.position - transform.position).sqrMagnitude < (enemy3.position - transform.position).sqrMagnitude)
                    {
                        enemy3 = ps3.transform;
                    }
                }
            }

            foreach (BlockAtivate activate in blockAtivates)
            {
                activate.EnemySetRange3(enemy3);
            }

            ps4 = Physics2D.OverlapCircle(transform.position, 7 * rangeInc, enemyLayerMask);
            if (ps4 == null)
            {
                enemy4 = null;
            }
            else
            {
                if (enemy4 == null)
                {
                    enemy4 = ps4.transform;
                }
                else
                {
                    if ((ps4.transform.position - transform.position).sqrMagnitude < (enemy4.position - transform.position).sqrMagnitude)
                    {
                        enemy4 = ps4.transform;
                    }
                }
            }

            foreach (BlockAtivate activate in blockAtivates)
            {
                activate.EnemySetRange4(enemy4);
            }

            ps5 = Physics2D.OverlapCircle(transform.position, 8 * rangeInc, enemyLayerMask);
            if (ps5 == null)
            {
                enemy5 = null;
            }
            else
            {
                if (enemy5 == null)
                {
                    enemy5 = ps5.transform;
                }
                else
                {
                    if ((ps5.transform.position - transform.position).sqrMagnitude < (enemy5.position - transform.position).sqrMagnitude)
                    {
                        enemy5 = ps5.transform;
                    }
                }
            }

            foreach (BlockAtivate activate in blockAtivates)
            {
                activate.EnemySetRange5(enemy5);
            }

            yield return seconds;
        }
    }
}
