using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    TMP_Text survivedTime, scoreTxt;

    public Player player;

    public GameObject[] blocks, attackBlocks, 
        enemyLv1Prefab, enemyLv2Prefab, enemyLv3Prefab, enemyLv4Prefab,
        enemyBossPrefab, enemyBossEnemyPrefab;
    [SerializeField]
    GameObject startWindow, pauseWindow, 
        getExpPrefab, gameoverWindow, stationPrefab;

    public bool onGame;
    bool sceneLoad;

    int enemyMass, enemyLv, enemyBossLv = 1, stationMass;
    int blockDestroyed, blockCombined, enemyDestroyed, stationDestroyed;
    int getExp;
    int m, s, ms;
    float totalTime;

    Vector3 blockTmp;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 0;
        pauseWindow.SetActive(false);
        gameoverWindow.SetActive(false);
    }

    private void Update()
    {
        if (onGame)
        {
            totalTime += Time.deltaTime;
            m = (int)(totalTime / 60);
            s = (int)(totalTime % 60);
            ms = (int)(Mathf.Floor(totalTime % 60 * 100f) % 100);
            if (s < 10)
            {
                if (ms < 10) survivedTime.text = $"{m}:0{s}.0{ms}";
                else survivedTime.text = $"{m}:0{s}.{ms}";
            }
            else
            {
                if (ms < 10) survivedTime.text = $"{m}:{s}.0{ms}";
                else survivedTime.text = $"{m}:{s}.{ms}";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onGame = false;
            Time.timeScale = 0;
            pauseWindow.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (enemyBossLv < 16)
        {
            if (m >= enemyBossLv * 2)
            {
                blockTmp = new Vector3(Random.Range(1f, 2f) * (Random.Range(0, 2) - 0.5f) * 10, Random.Range(1f, 2f) * (Random.Range(0, 2) - 0.5f) * 10, 0);
                Instantiate(enemyBossPrefab[enemyBossLv - 1], blockTmp, Quaternion.Euler(0, 0, 0));
                enemyBossLv++;
            }
        }
    }

    public void GetExp(int expV)
    {
        getExp += expV;
        //GameObject tmp = Instantiate(getExpPrefab);
        //tmp.GetComponentInChildren<TMP_Text>().text = $"Exp+{expV}";
        //Destroy(tmp, 0.6f);
    }

    public void BlockDestroyed()
    {
        blockDestroyed++;
        GetExp(15);
    }

    public void BlockCombined()
    {
        blockCombined++;
        GetExp(10);
    }

    public void EnemyDestroyed()
    {
        enemyDestroyed++;
        enemyMass--;
        GetExp(50);

        if (enemyMass < 5)
        {
            EnemyGenerate();
        }
    }

    public void StationDestroyed()
    {
        stationDestroyed++;
        stationMass--;
        GetExp(50);
    }

    IEnumerator StationGenerate()
    {
        WaitForSeconds seconds = new WaitForSeconds(10);

        while (true)
        {
            if (stationMass < 3)
            {
                if (player == null) StopCoroutine(StationGenerate());

                blockTmp = new Vector3(player.transform.position.x + Random.Range(1f, 20f) * (Random.Range(0, 2) - 0.5f) * 2, player.transform.position.y + Random.Range(1f, 20f) * (Random.Range(0, 2) - 0.5f) * 2, 0);
                Instantiate(stationPrefab, blockTmp, Quaternion.Euler(0, 0, 0));
                stationMass++;
            }

            yield return seconds;
        }
    }

    IEnumerator EnemyGenerateDelay()
    {
        yield return new WaitForSeconds(20);

        int mass = 5;

        for (int i = 0; i < mass; i++)
        {
            EnemyGenerate();
        }
    }

    public void EnemyGenerate()
    {
        blockTmp = new Vector3(Random.Range(1f, 2f) * (Random.Range(0, 2) - 0.5f) * 40, Random.Range(1f, 2f) * (Random.Range(0, 2) - 0.5f) * 40, 0);
        
        if (m > 4 && Random.Range(0,2) == 1)
        {
            Instantiate(enemyBossEnemyPrefab[Random.Range(0, Mathf.Clamp(m / 5, 0, enemyBossEnemyPrefab.Length))], blockTmp, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            int tmp = Random.Range(0, 4);
            switch (tmp)
            {
                case 0:
                    Instantiate(enemyLv1Prefab[Random.Range(0, enemyLv1Prefab.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
                    break;

                case 1:
                    Instantiate(enemyLv2Prefab[Random.Range(0, enemyLv2Prefab.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
                    break;

                case 3:
                    Instantiate(enemyLv3Prefab[Random.Range(0, enemyLv3Prefab.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
                    break;

                case 4:
                    Instantiate(enemyLv4Prefab[Random.Range(0, enemyLv4Prefab.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
                    break;
            }
        }
        enemyMass++;
    }

    public void GameStart()
    {
        int playerBlockMass = PlayerData.Instance.Lv;

        blockTmp = new Vector3(Random.Range(1f, 5f) * (Random.Range(0, 2) - 0.5f) * 2, Random.Range(1f, 5f) * (Random.Range(0, 2) - 0.5f) * 2, 0);
        Instantiate(attackBlocks[Random.Range(0, attackBlocks.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
        
        for (int i = 0; i < playerBlockMass; i++)
        {
            blockTmp = new Vector3(Random.Range(1f, 5f) * (Random.Range(0, 2) - 0.5f) * 2, Random.Range(1f, 5f) * (Random.Range(0, 2) - 0.5f) * 2, 0);
            Instantiate(blocks[Random.Range(0, blocks.Length)], blockTmp, Quaternion.Euler(0, 0, 0));
        }

        for (int i = 0; i < 3; i++)
        {
            blockTmp = new Vector3(player.transform.position.x + Random.Range(1f, 20f) * (Random.Range(0, 2) - 0.5f) * 2, player.transform.position.y + Random.Range(1f, 20f) * (Random.Range(0, 2) - 0.5f) * 2, 0);
            Instantiate(stationPrefab, blockTmp, Quaternion.Euler(0, 0, 0));
            stationMass++;
        }

        StartCoroutine(EnemyGenerateDelay());

        StartCoroutine(StationGenerate());

        Time.timeScale = 1;
        onGame = true;
        Destroy(startWindow);
    }

    public void GameOver()
    {
        if (!onGame) return;
        StopAllCoroutines();
        onGame = false;
        gameoverWindow.SetActive(true);
        string survivedStr = $"{survivedTime.text}\n";
        string stationStr = $"{stationDestroyed}\n";
        string enemyStr = $"{enemyDestroyed}\n\n";
        string blockComStr = $"{blockCombined}\n";
        string blockDesStr = $"{blockDestroyed}\n\n\n";
        string expStr = $"{getExp + (int)(totalTime / 10)}\n";
        if (PlayerData.Instance.GameOver(m, s, ms))
        {
            survivedStr = $"<color=#ff0000><size=25>New</size></color> {survivedTime.text}\n";
            //Debug.Log("New survive record");
        }
        if (PlayerData.Instance.GameOver(0, stationDestroyed))
        {
            stationStr = $"<color=#ff0000><size=25>New</size></color> {stationDestroyed}\n";
            //Debug.Log("New station destroy record");
        }
        if (PlayerData.Instance.GameOver(1, enemyDestroyed))
        {
            enemyStr = $"<color=#ff0000><size=25>New</size></color> {enemyDestroyed}\n\n";
            //Debug.Log("New enemy destroy record");
        }
        if (PlayerData.Instance.GameOver(2, blockCombined))
        {
            blockComStr = $"<color=#ff0000><size=25>New</size></color> {blockCombined}\n";
            //Debug.Log("New block combine record");
        }
        if (PlayerData.Instance.GameOver(3, blockDestroyed))
        {
            blockDesStr = $"<color=#ff0000><size=25>New</size></color> {blockDestroyed}\n\n\n";
            //Debug.Log("New block destroy record");
        }
        int lvTmp = PlayerData.Instance.GameOver(getExp + (int)(totalTime / 10));
        if (lvTmp > 0)
        {
            expStr = $"<color=#ff0000><size=25>Lv Up!</size></color> {getExp + (int)(totalTime/10)}\n";
            //Debug.Log($"Level up! {lvTmp} times");
        }
        scoreTxt.text = survivedStr + stationStr + enemyStr + blockComStr + blockDesStr + expStr;
    }

    public void Resume()
    {
        onGame = true;
        Time.timeScale = 1;
        pauseWindow.SetActive(false);
    }

    public void GameAgain()
    {
        if (sceneLoad) return;
        sceneLoad = true;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

    public void GameExit()
    {
        if (sceneLoad) return;
        sceneLoad = true;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
}
