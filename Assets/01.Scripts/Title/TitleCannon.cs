using System.Collections;
using UnityEngine;

public class TitleCannon : MonoBehaviour
{
    public Title title;

    public Transform[] enemyArr;
    [SerializeField]
    Transform bodyTrans;
    [SerializeField]
    ParticleSystem fireSmoke;
    [SerializeField]
    AudioSource fireSource;
    [SerializeField]
    GameObject bulletPrefab;
    Animator animator;
    int enemyNum;
    bool fire, canFire = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!fire)
        {
            bodyTrans.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        Vector2 tmp = bodyTrans.position - enemyArr[enemyNum].position;
        float rad = Mathf.Atan2(tmp.y, tmp.x);
        float angle = rad * Mathf.Rad2Deg;
        bodyTrans.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (canFire)
        {
            StartCoroutine(FireDelay());
        }
    }

    public void AttackEnemy(int enemyNumV)
    {
        enemyNum = enemyNumV;
        fire = true;
    }

    public void AttackCancle()
    {
        fire = false;
    }

    public void Fire()
    {
        fireSmoke.Play();
        GameObject tmp = Instantiate(bulletPrefab, bodyTrans);
        tmp.layer = 10;
        tmp.transform.localPosition = new Vector3(0, 0.72f, 0);
        if (!title.noneSound) fireSource.Play();
    }

    IEnumerator FireDelay()
    {
        canFire = false;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(5);

        canFire = true;
    }
}
