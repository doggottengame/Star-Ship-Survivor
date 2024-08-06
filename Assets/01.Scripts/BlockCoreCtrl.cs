using System;
using UnityEngine;

public class BlockCoreCtrl : MonoBehaviour
{
    [SerializeField]
    protected GameObject mainEngine;

    [SerializeField]
    protected AudioSource audioSource;

    public Vector3 vel;

    protected Rigidbody2D rb;

    public float speed = 15;
    public float xSpeed = 1;
    public float ySpeed = 1;

    protected Engine[] engines;

    protected bool onFire;

    public Transform blockTrans;
    public LayerMask enemyLayerMask, enemyShieldLayerMask;
    public int layerNum, droneLayer, playerAttackLayer, shieldLayer;
    public int blockMass, engineMass, radarMass, energyGeneratorMass, armoryMass;

    public virtual void BlockCombine(string blockName, GameObject block) { }//, GameObject conObj) {}
    public virtual void RepairMassGain(int mass) { }
    public virtual void BlockMassChanged() { }
}
