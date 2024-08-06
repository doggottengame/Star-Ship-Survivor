using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceStation : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 30);
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null && SceneManager.GetActiveScene().name == "02.PlayScene")
        {
            if (!GameManager.instance.onGame) return;
            Vector3 posTmp;
            int tmp = Random.Range(1, 3);
            for (int i = 0; i < tmp; i++)
            {
                posTmp = new Vector3(transform.position.x + Random.Range(1f, 3f) * (Random.Range(0, 2) - 0.5f) * 2,
                    transform.position.y + Random.Range(1f, 3f) * (Random.Range(0, 2) - 0.5f) * 2, 0);
                Instantiate(GameManager.instance.blocks[Random.Range(0, GameManager.instance.blocks.Length)],
                    posTmp, Quaternion.Euler(0, 0, 0));
            }

            try
            {
                GameManager.instance.player.RepairMassGain(100);
                GameManager.instance.StationDestroyed();
            }
            catch { }
        }
    }
}
