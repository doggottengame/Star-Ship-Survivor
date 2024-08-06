using UnityEngine;

public class ArmoryBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Armory", gameObject);

        Destroy(this);
        try
        {
        } catch { }
    }
}
