using UnityEngine;

public class RepairBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Repair", gameObject);

            Destroy(this);
        } catch { }
    }
}
