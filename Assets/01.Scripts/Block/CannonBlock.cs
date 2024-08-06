using UnityEngine;

public class CannonBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponentInParent<BlockCoreCtrl>().BlockCombine("Cannon", gameObject);

            GetComponent<Animator>().SetTrigger("Docking");
            Destroy(this);
        } catch { }
    }
}
