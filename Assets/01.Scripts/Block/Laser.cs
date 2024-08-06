using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask enemyShieldLayer;
    [SerializeField]
    GameObject laserHitPrefab;
    GameObject laserHit;
    [SerializeField]
    AudioClip laserStarting, lasering;
    AudioSource laserSound;
    Transform laserGun, enemy;
    DamageMass damageMass;
    float enemyDis = 0;
    RaycastHit2D shieldHit;
    bool set, hitOnShield;
    BlockCoreCtrl blockCoreCtrl;

    private void Awake()
    {
        laserSound = GetComponent<AudioSource>();
        damageMass = GetComponent<DamageMass>();
        blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();

        laserSound.PlayOneShot(laserStarting);
    }

    private void FixedUpdate()
    {
        if (!set) return;
        if (enemy == null || !enemy.gameObject.activeSelf || laserGun == null)
        {
            Destroy(gameObject);
            return;
        }
        enemyDis = (enemy.position - laserGun.position).magnitude;

        shieldHit = Physics2D.Raycast(laserGun.position, enemy.position - laserGun.position, enemyDis, enemyShieldLayer);
        if (shieldHit.collider != null)
        {
            enemyDis = shieldHit.distance;
            laserHit.transform.position = shieldHit.point;
            hitOnShield = true;
        }
        else
        {
            laserHit.transform.position = enemy.position;
            hitOnShield = false;
        }
        
        transform.localScale = new Vector3(0.1f, enemyDis - 0.5f, 1);
        transform.localPosition = new Vector3(0, (enemyDis - 0.5f) / 2 + 0.5f, 1);
    }

    public void Set(Transform enemyV, Transform laserGunV)
    {
        enemy = enemyV;
        if (laserGun == null)
        {
            laserHit = Instantiate(laserHitPrefab, transform.position, Quaternion.identity, transform);
            StartCoroutine(Lasering());
        }
        laserGun = laserGunV;
        set = true;
    }
    
    IEnumerator Lasering()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(0.2f);

        while (true)
        {
            if (enemy == null)
            {
                StopCoroutine(Lasering());
                Destroy(gameObject);
            }
            else
            {
                laserSound.PlayOneShot(lasering);
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
