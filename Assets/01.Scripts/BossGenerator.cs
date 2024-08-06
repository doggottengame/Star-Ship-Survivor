using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public Transform[] ct;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in ct)
        {
            t.SetParent(null);
        }
        Destroy(gameObject);
    }
}
