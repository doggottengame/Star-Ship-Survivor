using UnityEngine;

public class ShotgunBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Shotgun", gameObject);

            Destroy(this);
        }
        catch { }
    }
}
