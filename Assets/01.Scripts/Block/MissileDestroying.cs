using UnityEngine;

public class MissileDestroying : MonoBehaviour
{
    [SerializeField]
    GameObject destroyedPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(destroyedPrefab, transform.position, transform.rotation);

        Destroy(transform.parent.gameObject);
    }
}
