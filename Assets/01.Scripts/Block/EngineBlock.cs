using UnityEngine;

public class EngineBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Engine", gameObject);


            Destroy(this);
        } catch { }
    }
}
