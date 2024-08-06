using System.Collections;
using UnityEngine;

public class DroneDockBlockActivate : BlockAtivate
{
    BlockCoreCtrl blockCoreCtrl;
    GameObject[] drones = new GameObject[3];
    [SerializeField]
    GameObject dronePrefab;
    Transform enemy;
    LayerMask enemyLayerMask;
    int droneLayer, playerAttackLayer;
    int droneMaxMass = 3;
    int droneMass;
    bool setAttack;

    private void Awake()
    {
        blockSet = GetComponent<BlockSet>();
    }

    public void Update()
    {
        if (enemy == null)
        {
            if (setAttack)
            {
                setAttack = false;
                StopCoroutine(DroneStartAttack());
            }
            if (drones[0] != null && drones[0].activeSelf)
            {
                for (int i = 0; i < drones.Length; i++)
                {
                    drones[i].GetComponent<Drone1>().ComeBackCall();
                }
            }
            return;
        }
        else
        {
            if (!setAttack)
            {
                setAttack = true;
                StartCoroutine(DroneStartAttack());
            }
        }
    }

    public override void Set()
    {
        if (!activated)
        {
            activated = true;
            droneMass = droneMaxMass;

            enemyLayerMask = blockSet.GetEnemyLayerMask();
            droneLayer = blockSet.GetDroneLayer();
            playerAttackLayer = blockSet.GetAttackLayer();
            blockCoreCtrl = GetComponentInParent<BlockCoreCtrl>();

            for (int i = 0; i < 3; i++)
            {
                drones[i] = Instantiate(dronePrefab, transform);
                drones[i].transform.position = transform.position;
                drones[i].GetComponent<Drone1>().Set(this, transform, droneLayer, playerAttackLayer, i, blockCoreCtrl.armoryMass);
                drones[i].SetActive(false);
            }
        }
    }

    public override void EnemySetRange5(Transform enemyV)
    {
        enemy = enemyV;
    }

    IEnumerator DroneStartAttack()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        for (int i = 0; i < droneMass; i++)
        {
            drones[i].SetActive(true);
            drones[i].transform.position = transform.position;
            drones[i].GetComponent<Drone1>().EnemyLockOn(enemy, blockCoreCtrl.armoryMass);

            yield return seconds;
        }
    }

    public void DroneDestroyed(int serialNumV)
    {
        try
        {
            drones[serialNumV] = Instantiate(dronePrefab, transform);
            drones[serialNumV].transform.position = transform.position;
            drones[serialNumV].GetComponent<Drone1>().Set(this, transform, droneLayer, playerAttackLayer, serialNumV, blockCoreCtrl.armoryMass);
            drones[serialNumV].SetActive(false);
            droneMass--;
            StartCoroutine(DroneGenerate());
        }
        catch { }
    }

    IEnumerator DroneGenerate()
    {
        yield return new WaitForSeconds(5);
        droneMass++;
    }
}
