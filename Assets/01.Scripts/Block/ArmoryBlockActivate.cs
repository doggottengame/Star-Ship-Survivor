public class ArmoryBlockActivate : BlockAtivate
{
    BlockCoreCtrl blockCoreCtrl;
    int layerNum;

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
            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();
            blockCoreCtrl.armoryMass++;
        }
    }

    public void OnDestroy()
    {
        if (GameManager.instance == null || !GameManager.instance.onGame) return;
        if (blockCoreCtrl != null) blockCoreCtrl.armoryMass--;
    }
}
