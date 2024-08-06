using System.Collections;
using UnityEngine;

public class Plasma1Block : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public LayerMask enemyShieldLayer;
    public int layerNum;
    [SerializeField]
    Transform enemy, plasmaBodyTrans;
    [SerializeField]
    GameObject plasmaPrefab;
    GameObject plasma;

    bool dockingComplete;

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
        if (!dockingComplete) return;
        if (enemy == null || plasma == null)
        {
            plasmaBodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
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

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        while (true)
        {
            if (dockingComplete)
            {
                Collider2D ps = Physics2D.OverlapCircle(transform.position, 4, enemyLayerMask);
                if (ps == null)
                {
                    enemy = null;
                }
                else
                {
                    if ((enemy == null || (enemy != ps.transform && (ps.transform.position - transform.position).sqrMagnitude < (enemy.position - transform.position).sqrMagnitude)) && ps.gameObject.activeSelf)
                    {
                        enemy = ps.transform;
                        if (plasma == null)
                        {
                            plasma = Instantiate(plasmaPrefab, plasmaBodyTrans);
                            plasma.layer = layerNum;
                        }
                        plasma.GetComponent<Plasma>().enemyShieldLayer = enemyShieldLayer;
                        plasma.GetComponent<Plasma>().Set(enemy, plasmaBodyTrans);
                    }
                }
            }

            yield return seconds;
        }
    }

    public void DockingComplete()
    {
        dockingComplete = true;
    }
}
