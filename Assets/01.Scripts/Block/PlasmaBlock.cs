using UnityEngine;

public class PlasmaBlock : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Plasma", gameObject);

            Destroy(this);
        } catch { }
    }
}
