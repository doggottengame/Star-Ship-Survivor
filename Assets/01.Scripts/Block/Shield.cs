using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    string blockType;
    [SerializeField]
    List<string> decDmgType;
    BlockCoreCtrl blockCoreCtrl;
    [SerializeField]
    AudioClip shieldOpenClip, shieldAttackedClip;
    [SerializeField]
    AudioSource shieldSound;
    SpriteRenderer spriteRenderer;
    int shieldLayer;
    public float shieldMaxMass;
    float shieldMass;
    bool shieldDestroyed;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DamageMass>() != null)
        {
            DamageMass dmgMass;
            dmgMass = collision.GetComponent<DamageMass>();
            Damaged(dmgMass.dmg, dmgMass.dmgType);
        }
    }

    public void Damaged(float dmgMass, string dmgTypeV)
    {
        if (decDmgType.Contains(dmgTypeV))
        {
            shieldMass -= dmgMass / 2;
        }
        else if (blockType == dmgTypeV)
        {
            shieldMass -= dmgMass * 2;
        }
        else
        {
            shieldMass -= dmgMass;
        }
        animator.SetTrigger("Attacked");
        shieldSound.PlayOneShot(shieldAttackedClip);
        if (blockType == dmgTypeV)
        {
            shieldMass -= dmgMass;
        }

        if (shieldMass <= 0)
        {
            DeActivateShield();
            animator.SetTrigger("Destroyed");
        }
    }

    IEnumerator ShieldGenerate()
    {
        WaitForSeconds seconds = new WaitForSeconds(1);

        while (true)
        {
            if (shieldMass + 5 + blockCoreCtrl.energyGeneratorMass <= shieldMaxMass + blockCoreCtrl.energyGeneratorMass * 50) shieldMass += 5 + blockCoreCtrl.energyGeneratorMass;
            else
            {
                shieldMass = shieldMaxMass + blockCoreCtrl.energyGeneratorMass * 50;
                if (shieldDestroyed)
                {
                    ActivateShield();
                }
            }

            yield return seconds;
        }
    }

    public void Set()
    {
        shieldLayer = GetComponentInParent<BlockSet>().GetShieldLayer();
        blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();

        StartCoroutine(ShieldGenerate());
        ActivateShield();
    }

    public void DeActivateShield()
    {
        shieldDestroyed = true;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.enabled = false;
        gameObject.layer = 18;
    }

    public void ActivateShield()
    {
        shieldDestroyed = false;

        animator.SetTrigger("Generated");
        shieldSound.PlayOneShot(shieldOpenClip);

        shieldMass = shieldMaxMass + blockCoreCtrl.energyGeneratorMass * 50;
        spriteRenderer.enabled = true;
        gameObject.layer = shieldLayer;
    }
}
