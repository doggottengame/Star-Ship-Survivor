using UnityEngine;

public class MissileBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Missile", gameObject);

            Destroy(this);
        } catch { }
    }
}
