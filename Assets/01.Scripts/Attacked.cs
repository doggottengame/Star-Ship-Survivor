using UnityEngine;

public class Attacked : MonoBehaviour
{
    protected float hp = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damaged(collision.GetComponent<DamageMass>().dmg);
    }

    public virtual void Damaged(float dmgMass)
    {
        hp -= dmgMass;
        Debug.Log($"Damaged {dmgMass}\nnow hp: {hp}");
        if (hp < 0)
        {
            Debug.Log("Die");
            Destroy(gameObject);
        }
    }
}
