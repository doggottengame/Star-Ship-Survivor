using System.Collections;
using UnityEngine;

public class Drone1 : MonoBehaviour
{
    [SerializeField]
    AudioSource droneAudio;
    [SerializeField]
    GameObject bulletPrefab;
    Transform[] bullets = new Transform[4];
    Transform dockTrans;
    Transform enemyTrans;
    Vector3 bulletScale = new Vector3(0.02f, 0.1f, 1);
    Rigidbody2D rb;
    DroneDockBlockActivate dock;
    BlockAttacked blockAttacked;
    int enemyLayer, attackLayer, serialNum, armory;
    float speed = 10, dmg;
    bool set, targetOn, onAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        blockAttacked = GetComponent<BlockAttacked>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!set) return;
        if (targetOn) AttackTarget();
        else ComeBack();
        if (dockTrans == null) Destroy(gameObject);
    }

    public void Set(DroneDockBlockActivate dockV, Transform dockTransV, int enemyLayerV, int attackLayerV, int serialNumV, int armoryV)
    {
        dock = dockV;
        dockTrans = dockTransV;
        enemyLayer = enemyLayerV;
        gameObject.layer = enemyLayer;
        attackLayer = attackLayerV;
        serialNum = serialNumV;
        armory = armoryV;

        dmg = 2 + armory * 0.5f;
        if (blockAttacked != null)
        {
            blockAttacked.maxHp += armory * 10;
            blockAttacked.Repair(blockAttacked.maxHp);
        }

        set = true;
    }

    public void EnemyLockOn(Transform enemyTransV, int armoryV)
    {
        transform.SetParent(null);
        enemyTrans = enemyTransV;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = false;
        targetOn = true;
        armory = armoryV;

        dmg = 2 + armory * 0.5f;
        blockAttacked.maxHp += armory * 10;
        blockAttacked.Repair(blockAttacked.maxHp);
    }

    public void ComeBackCall()
    {
        targetOn = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        rb.freezeRotation = true;
    }

    void AttackTarget()
    {
        if (enemyTrans == null)
        {
            onAttack = false;
            ComeBack();
            return;
        }
        Vector2 tmp = transform.position - enemyTrans.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        if ((transform.position - enemyTrans.position).sqrMagnitude > 1 && rb.velocity.magnitude < speed)
        {
            rb.AddForce((enemyTrans.position - transform.position) * 50 * Time.deltaTime);
        }
        else
        {
            if (!onAttack)
            {
                onAttack = true;
                StartCoroutine(Attacking());
            }
        }
    }

    void ComeBack()
    {
        if (onAttack)
        {
            onAttack = false;
            StopCoroutine(Attacking());
        }
        if (dockTrans == null || transform == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 tmp = transform.position - dockTrans.position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        transform.position = Vector3.Lerp(transform.position, dockTrans.position, Time.deltaTime);

        if ((transform.position - dockTrans.position).sqrMagnitude < 1)
        {
            transform.SetParent(dockTrans);
            GetComponent<BlockAttacked>().HpReset();
            if (droneAudio != null) droneAudio.Stop();
            gameObject.SetActive(false);
        }
    }

    IEnumerator Attacking()
    {
        WaitForSeconds seconds = new WaitForSeconds(1);

        while (onAttack)
        {
            bullets[0] = Instantiate(bulletPrefab, transform).transform;
            bullets[0].localPosition = new Vector3(0.345f, 0.45f, 0);
            bullets[0].localScale = bulletScale;
            bullets[0].GetComponent<DamageMass>().dmg = dmg;
            bullets[0].gameObject.layer = attackLayer;

            bullets[1] = Instantiate(bulletPrefab, transform).transform;
            bullets[1].localPosition = new Vector3(0.175f, 0.35f, 0);
            bullets[1].localScale = bulletScale;
            bullets[1].GetComponent<DamageMass>().dmg = dmg;
            bullets[1].gameObject.layer = attackLayer;

            bullets[2] = Instantiate(bulletPrefab, transform).transform;
            bullets[2].localPosition = new Vector3(-0.345f, 0.45f, 0);
            bullets[2].localScale = bulletScale;
            bullets[2].GetComponent<DamageMass>().dmg = dmg;
            bullets[2].gameObject.layer = attackLayer;

            bullets[3] = Instantiate(bulletPrefab, transform).transform;
            bullets[3].localPosition = new Vector3(-0.175f, 0.35f, 0);
            bullets[3].localScale = bulletScale;
            bullets[3].GetComponent<DamageMass>().dmg = dmg;
            bullets[3].gameObject.layer = attackLayer;
            if (droneAudio != null) droneAudio.Play();

            yield return seconds;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance == null || !GameManager.instance.onGame) return;
        if (dock != null)
        {
            dock.DroneDestroyed(serialNum);
            onAttack = false;
            StopCoroutine(Attacking());
        }
    }
}
