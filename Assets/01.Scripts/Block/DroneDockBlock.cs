using UnityEngine;

public class DroneDockBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("DroneDock", gameObject);

            Destroy(this);
        } catch { }
    }
}
