using UnityEngine;

public class Engine : BlockAtivate
{
    [SerializeField]
    ParticleSystem engine;
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

            blockCoreCtrl.engineMass++;
            FireEngine();
        }
    }

    public void FireEngine()
    {
        engine.Play();
    }

    public void StopEngine()
    {
        engine.Stop();
    }

    private void OnDestroy()
    {
        if (GameManager.instance == null || !GameManager.instance.onGame) return;
        if (blockCoreCtrl != null) blockCoreCtrl.engineMass--;
    }
}
