using UnityEngine;

public class Steel1Block : MonoBehaviour
{
    [SerializeField]
    AudioClip steelCollisonClip;
    AudioSource audioSource;
    [SerializeField]
    GameObject hitSpark;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<BlockAttacked>().hitAction = SteelCollison;
    }

    public void SteelCollison(Vector3 hitPosV, Vector3 hitRotV)
    {
        audioSource.PlayOneShot(steelCollisonClip);
        hitRotV = new Vector3(0, 0, hitRotV.z + 180);
        Instantiate(hitSpark, hitPosV, Quaternion.Euler(hitRotV));
    }
}