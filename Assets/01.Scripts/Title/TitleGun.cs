using UnityEngine;

public class TitleGun : MonoBehaviour
{
    public Title title;

    public Transform[] enemyArr;
    [SerializeField]
    Transform bodyTrans;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    AudioSource fireSource;
    Animator animator;
    int enemyNum;
    bool fire;

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
    }

    public void AttackEnemy(int enemyNumV)
    {
        enemyNum = enemyNumV;
        fire = true;
        animator.SetBool("Attack", true);
    }

    public void AttackCancle()
    {
        fire = false;
        animator.SetBool("Attack", false);
    }

    public void FireR()
    {
        if (!fire) return;
        GameObject tmp = Instantiate(bulletPrefab, bodyTrans);
        tmp.layer = 10;
        tmp.transform.localPosition = new Vector3(0.05f, 0.55f, 0);
        if (!title.noneSound) fireSource.Play();
    }

    public void FireL()
    {
        if (!fire) return;
        GameObject tmp = Instantiate(bulletPrefab, bodyTrans);
        tmp.layer = 10;
        tmp.transform.localPosition = new Vector3(-0.05f, 0.55f, 0);
        if (!title.noneSound) fireSource.Play();
    }
}
