using UnityEngine;

public class GunBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Gun", gameObject);

            Destroy(this);
        } catch { }
    }
}
