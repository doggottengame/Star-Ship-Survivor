using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    GameObject bulletMeltingPrefab;
    DamageMass damageMass;
    [SerializeField]
    float forcePower;

    // Start is called before the first frame update
    void Start()
    {
        damageMass = GetComponent<DamageMass>();
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.up * forcePower);

        Destroy(gameObject, 2);
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 13)
        {
            Instantiate(bulletMeltingPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
