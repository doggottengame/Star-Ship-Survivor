using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Shield", gameObject);

            Destroy(this);
        } catch { }
    }
}
