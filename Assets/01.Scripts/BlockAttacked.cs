using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttacked : MonoBehaviour
{
    public Action<Vector3, Vector3> hitAction;
    [SerializeField]
    string blockType;
    [SerializeField]
    List<string> decDmgType;
    [SerializeField]
    GameObject destroyedPrefab, mainCore;
    [SerializeField]
    Transform hpEff;
    public int blockNum;
    public float maxHp = 10;
    protected float hp;
    public bool isPlayer, isMainCore;

    private void Awake()
    {
        hp = maxHp;
        if (hpEff != null)
        {
            hpEff.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";
            hpEff.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DamageMass>() != null)
        {
            DamageMass dmgMass;
            dmgMass = collision.GetComponent<DamageMass>();
            Damaged(collision.GetComponent<DamageMass>().dmg, collision.GetComponent<DamageMass>().dmgType);
            collision.GetComponent<DamageMass>().dmg *= 0.8f;
            if (hitAction != null) hitAction(collision.transform.position, collision.transform.eulerAngles);
        }
    }

    public void Damaged(float dmgMass, string dmgTypeV)
    {
        //Debug.Log($"Dmg mass: {dmgMass}\nThis Name: {gameObject.name}");
        if (decDmgType.Contains(dmgTypeV))
        {
            hp -= dmgMass / 2;
        }
        else if (blockType == dmgTypeV)
        {
            hp -= dmgMass * 2;
        }
        else
        {
            hp -= dmgMass;
        }

        if (hpEff != null)
        {
            hpEff.localPosition = new Vector3(0, -0.5f * hp / maxHp, 0);
            hpEff.localScale = new Vector3(1, 1 - hp / maxHp, 1);
        }

        if (hp <= 0)
        {
            Instantiate(destroyedPrefab, transform.position, transform.rotation);

            if (isPlayer)
            {
                GameManager.instance.GameOver();
                Destroy(mainCore);
            }
            else
            {
                if (isMainCore)
                {
                    if (gameObject.layer == 9 || gameObject.layer == 15)
                    {
                        GameManager.instance.EnemyDestroyed();
                    }

                    Destroy(mainCore);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public float Repair(float repairMass)
    {
        if (repairMass + hp >= maxHp)
        {
            float tmp = maxHp - hp;
            hp = maxHp;
            return repairMass - tmp;
        }
        else
        {
            hp += repairMass;
            return repairMass;
        }
    }

    public void HpReset()
    {
        hp = maxHp;
    }

    private void OnDestroy()
    {
        if (isMainCore || GameManager.instance == null || !GameManager.instance.onGame) return;
        
        if (blockNum != 0)
        {
            int per = UnityEngine.Random.Range(0, 10);
            if (per < 3)
            {
                Instantiate(GameManager.instance.blocks[blockNum - 1], transform.position, transform.rotation);
            }
        }

        if (gameObject.layer == 9)
        {
            GameManager.instance.BlockDestroyed();
        }
    }
}
