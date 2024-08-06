using UnityEngine;

public class RailGunBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("RailGun", gameObject);

            Destroy(this);
        } catch { }
    }
}
