using System.Collections;
using UnityEngine;

public class DroneDock1Block : MonoBehaviour
{
    [SerializeField]
    GameObject[] drones;
    [SerializeField]
    GameObject dronePrefab;
    public LayerMask enemyLayerMask;
    Transform enemy;
    public int droneLayer, playerAttackLayer;
    int droneMaxMass;
    int droneMass;

    private void OnEnable()
    {
        droneMaxMass = drones.Length;
        droneMass = droneMaxMass;

        for (int i = 0; i < droneMass; i++)
        {
            drones[i].SetActive(false);
            //drones[i].GetComponent<Drone1>().Set(this, transform, droneLayer, playerAttackLayer, i);
        }

        StartCoroutine(EnemySearch());
    }

    IEnumerator EnemySearch()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        while (true)
        {
            if (enemy == null)
            {
                Collider2D ps = Physics2D.OverlapCircle(transform.position, 6, enemyLayerMask);
                if (ps != null)
                {
                    enemy = ps.transform;
                    StartCoroutine(DroneStartAttack());
                }
            }
            else
            {
                if ((enemy.position - transform.position).sqrMagnitude > 49)
                {
                    if (drones[0] != null)
                    {
                        for (int i = 0; i < drones.Length; i++)
                        {
                            drones[i].GetComponent<Drone1>().ComeBackCall();
                        }
                        enemy = null;
                    }
                }
            }

            yield return seconds;
        }
    }

    IEnumerator DroneStartAttack()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);

        for (int i = 0; i < droneMass; i++)
        {
            drones[i].SetActive(true);
            drones[i].transform.position = transform.position;
            //drones[i].GetComponent<Drone1>().EnemyLockOn(enemy);

            yield return seconds;
        }
    }

    public void DroneDestroyed(int serialNumV)
    {
        drones[serialNumV] = Instantiate(dronePrefab, transform);
        drones[serialNumV].transform.position = transform.position;
        //drones[serialNumV].GetComponent<Drone1>().Set(this, transform, droneLayer, playerAttackLayer, serialNumV);
        drones[serialNumV].SetActive(false);
        droneMass--;
        StartCoroutine(DroneGenerate());
    }

    IEnumerator DroneGenerate()
    {
        yield return new WaitForSeconds(5);
        droneMass++;
    }
}
