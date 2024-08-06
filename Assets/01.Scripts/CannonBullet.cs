using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    [SerializeField]
    GameObject bulletMeltingPrefab;
    DamageMass damageMass;
    [SerializeField]
    float forcePower;
    [SerializeField]
    GameObject boomPrefab;

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
            Transform tmp = Instantiate(bulletMeltingPrefab, transform.position, transform.rotation).transform;
            tmp.localScale *= 2;
        }
        else
        {
            Instantiate(boomPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
