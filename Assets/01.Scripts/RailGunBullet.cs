using UnityEngine;

public class RailGunBullet : MonoBehaviour
{
    [SerializeField]
    GameObject bulletMeltingPrefab;
    [SerializeField]
    AudioClip penetrateClip;
    AudioSource audioSource;
    DamageMass damageMass;
    [SerializeField]
    float forcePower;
    bool fire;

    public void Fire()
    {
        transform.SetParent(null);
        damageMass = GetComponent<DamageMass>();
        gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.AddRelativeForce(Vector3.up * forcePower);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Destroy(gameObject, 1);
        fire = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fire) return;
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 13)
        {
            Instantiate(bulletMeltingPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            audioSource.PlayOneShot(penetrateClip);
        }
    }
}
