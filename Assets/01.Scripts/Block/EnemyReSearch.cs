using System.Collections;
using UnityEngine;

public class EnemyReSearch : MonoBehaviour
{
    [SerializeField]
    BlockSet enemySearchCtrl;
    [SerializeField]
    Animator animator;
    int searchLayer;

    bool set;

    public void Set(int searchLayerV)
    {
        searchLayer = searchLayerV;

        gameObject.layer = searchLayer;

        set = true;
        StartCoroutine(Search());
    }

    void SearchAgain()
    {
        animator.SetTrigger("Set");
    }

    IEnumerator Search()
    {
        WaitForSeconds seconds = new WaitForSeconds(1);
        while (true)
        {
            SearchAgain();

            yield return seconds;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!set) return;
        //if (enemySearchCtrl.enemy != null)
        //{
            //if ((enemySearchCtrl.enemy.position - transform.position).sqrMagnitude >
            //    (collision.transform.position - transform.position).sqrMagnitude)
            //{
            //    enemySearchCtrl.enemy = collision.transform;
            //}
        //}
        //else
        //{
            //enemySearchCtrl.enemy = collision.transform;
        //}
    }
}
