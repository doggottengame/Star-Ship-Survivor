using System.Collections;
using UnityEngine;

public class Plasma : MonoBehaviour
{
    public LayerMask enemyShieldLayer;
    [SerializeField]
    AudioClip plasmaStarting, plasmaing;
    AudioSource plasmaSound;
    Transform plasma, enemy;
    [SerializeField]
    GameObject plasmaHitPrefab;
    GameObject plasmaHit;
    DamageMass damageMass;
    float enemyDis = 0;
    RaycastHit2D shieldHit;
    bool set, hitOnShield;
    BlockCoreCtrl blockCoreCtrl;

    private void Awake()
    {
        plasmaSound = GetComponent<AudioSource>();
        damageMass = GetComponent<DamageMass>();
        blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();

        plasmaSound.PlayOneShot(plasmaStarting);
    }

    private void FixedUpdate()
    {
        if (!set) return;
        if (enemy == null || !enemy.gameObject.activeSelf || plasma == null)
        {
            Destroy(gameObject);
            return;
        }
        enemyDis = (enemy.position - plasma.position).magnitude;

        shieldHit = Physics2D.Raycast(plasma.position, enemy.position - plasma.position, enemyDis, enemyShieldLayer);
        if (shieldHit.collider != null)
        {
            enemyDis = shieldHit.distance;
            plasmaHit.transform.position = shieldHit.point;
            hitOnShield = true;
        }
        else
        {
            plasmaHit.transform.position = enemy.position;
            hitOnShield = false;
        }

        transform.localScale = new Vector3(1, enemyDis - 0.1f, 1);
        transform.localPosition = new Vector3(0, (enemyDis - 0.1f) / 2 + 0.1f, 1);
    }

    public void Set(Transform enemyV, Transform plasmaV)
    {
        enemy = enemyV;
        if (plasma == null)
        {
            plasmaHit = Instantiate(plasmaHitPrefab, transform.position, Quaternion.identity, transform);
            StartCoroutine(Plasmaing());
        }
        plasma = plasmaV;
        set = true;
    }

    IEnumerator Plasmaing()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(0.2f);

        while (true)
        {
            if (enemy == null)
            {
                StopCoroutine(Plasmaing());
                Destroy(gameObject);
            }
            else
            {
                plasmaSound.PlayOneShot(plasmaing);
                if (hitOnShield)
                {
                    shieldHit.collider.GetComponent<Shield>().Damaged(damageMass.dmg + blockCoreCtrl.energyGeneratorMass, damageMass.dmgType);
                }
                else
                {
                    if (enemy.GetComponent<BlockAttacked>() != null)
                    {
                        enemy.GetComponent<BlockAttacked>().Damaged(damageMass.dmg + blockCoreCtrl.energyGeneratorMass, damageMass.dmgType);
                    }
                    else
                    {
                        enemy.GetComponentInParent<BlockAttacked>().Damaged(damageMass.dmg + blockCoreCtrl.energyGeneratorMass, damageMass.dmgType);
                    }
                }
            }

            yield return seconds;
        }
    }
}
