using UnityEngine;

public class BlockSet : MonoBehaviour
{
    LayerMask enemyLayerMask;
    LayerMask enemyShieldLayer;
    int layerNum, droneLayer, playerAttackLayer, shieldLayer;

    public void Set(int layerNumV, int droneLayerV, int playerAttackLayerV, int shieldLayerV, LayerMask enemyLayerMaskV, LayerMask enemyShieldLayerV)
    {
        layerNum = layerNumV;
        droneLayer = droneLayerV;
        playerAttackLayer = playerAttackLayerV;
        shieldLayer = shieldLayerV;
        enemyLayerMask = enemyLayerMaskV;
        enemyShieldLayer = enemyShieldLayerV;
    }

    public int GetLayerNum()
    {
        return layerNum;
    }

    public int GetDroneLayer()
    {
        return droneLayer;
    }

    public int GetAttackLayer()
    {
        return playerAttackLayer;
    }

    public int GetShieldLayer()
    {
        return shieldLayer;
    }

    public LayerMask GetEnemyLayerMask()
    {
        return enemyLayerMask;
    }

    public LayerMask GetEnemyShieldLayer()
    {
        return enemyShieldLayer;
    }
}
