using UnityEngine;

public class Shield1Block : BlockAtivate
{
    [SerializeField]
    Shield shield;

    private void Awake()
    {
        shield.shieldMaxMass = 500;
        shield.DeActivateShield();
        blockSet = GetComponent<BlockSet>();
    }

    public override void Set()
    {
        if (!activated)
        {
            activated = true;
            shield.Set();
        }
    }
}
