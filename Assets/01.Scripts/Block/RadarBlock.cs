using UnityEngine;

public class RadarBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Radar", gameObject);

            Destroy(this);
        } catch { }
    }
}
