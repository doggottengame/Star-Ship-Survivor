using UnityEngine;

public class EnergyGeneratorBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("EnergyGenerator", gameObject);

            Destroy(this);
        } catch { }
    }
}
