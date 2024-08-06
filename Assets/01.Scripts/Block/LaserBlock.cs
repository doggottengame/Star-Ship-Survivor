using UnityEngine;

public class LaserBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Laser", gameObject);

            Destroy(this);
        } catch { }
    }
}
