public class RadarBlockActivate : BlockAtivate
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
            blockCoreCtrl.radarMass++;
        }
    }

    public void OnDestroy()
    {
        if (GameManager.instance == null || !GameManager.instance.onGame) return;
        if (blockCoreCtrl != null) blockCoreCtrl.radarMass--;
    }
}
