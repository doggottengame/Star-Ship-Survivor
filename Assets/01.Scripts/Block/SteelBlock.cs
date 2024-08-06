using UnityEngine;

public class SteelBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Steel1Block>().enabled = true;
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Steel", gameObject);

            Destroy(this);
        } catch { }
    }
}
