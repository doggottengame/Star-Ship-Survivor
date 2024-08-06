using UnityEngine;

public class Engine1Block : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Engine>().enabled = true;
    }
}
