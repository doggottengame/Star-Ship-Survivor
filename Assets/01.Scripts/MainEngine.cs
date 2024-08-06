using UnityEngine;

public class MainEngine : MonoBehaviour
{
    [SerializeField]
    ParticleSystem engine;

    public void FireEngine()
    {
        engine.Play();
    }

    public void StopEngine()
    {
        engine.Stop();
    }
}
