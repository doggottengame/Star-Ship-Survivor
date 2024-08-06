using UnityEngine;

public class BlockAtivate : MonoBehaviour
{
    public BlockSet blockSet;
    public bool activated;

    public virtual void Set() { }

    public virtual void EnemySetRange1(Transform enemyV) { }

    public virtual void EnemySetRange2(Transform enemyV) { }

    public virtual void EnemySetRange3(Transform enemyV) { }

    public virtual void EnemySetRange4(Transform enemyV) { }

    public virtual void EnemySetRange5(Transform enemyV) { }

    private void OnDestroy()
    {
        if (GetComponentInParent<BlockCoreCtrl>() != null)
        {
            GetComponentInParent<BlockCoreCtrl>().BlockMassChanged();
        }
    }
}
