using System.Collections;
using UnityEngine;

public class Repair : BlockAtivate
{
    [SerializeField]
    GameObject repairPrefab;
    [SerializeField]
    Transform blocksTrans;

    int layerNum;
    float repairMaxMass = 1000;
    float repairRemainMass;
    float repairMass;

    private void Awake()
    {
        blockSet = GetComponent<BlockSet>();
    }

    public override void Set()
    {
        if (!activated)
        {
            activated = true;
            layerNum = blockSet.GetLayerNum();
            gameObject.layer = layerNum;

            blocksTrans = transform.parent;
            repairRemainMass = repairMaxMass;
            StartCoroutine(Repairing());
        }
    }

    IEnumerator Repairing()
    {
        WaitForSeconds seconds = new WaitForSeconds(5);
        GameObject repairTmp;
        float repairMassTmp;

        BlockAttacked[] blockAttackeds;

        while (true)
        {
            blockAttackeds = blocksTrans.GetComponentsInChildren<BlockAttacked>();
            repairMass = repairRemainMass / blockAttackeds.Length;
            repairRemainMass = 0;

            foreach (BlockAttacked blocks in blockAttackeds)
            {
                repairMassTmp = blocks.Repair(repairMass);
                repairRemainMass += repairMassTmp;
                if (repairMassTmp < repairMass)
                {
                    repairTmp = Instantiate(repairPrefab, blocks.transform);
                    repairTmp.transform.localPosition = Vector3.zero;
                    Destroy(repairTmp, 1);
                }
            }

            yield return seconds;
        }
    }

    public void RepairMassGain(int mass)
    {
        repairMaxMass += mass;
        repairRemainMass += mass;
    }
}
