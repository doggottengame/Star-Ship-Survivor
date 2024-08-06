using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public TitleGun gun1, gun2;
    public TitleCannon cannon;
    public GameObject[] manualPage;
    public GameObject recordWindow, manualWindow;

    public Image ExpGauge;
    public TMP_Text Lv, Exp, MostSurvive, MostStationDestroyed, 
        MostEnemyDestroyed, MostBlockCombined, MostBlockDestroyed;

    public bool noneSound, windowOn;
    bool sceneLoad;

    private void Start()
    {
        WindowClose();
    }

    public void TargetingEnemy(int enemyNum)
    {
        gun1.AttackEnemy(enemyNum);
        gun2.AttackEnemy(enemyNum);
        cannon.AttackEnemy(enemyNum);
    }

    public void TargetingCancle()
    {
        gun1.AttackCancle();
        gun2.AttackCancle();
        cannon.AttackCancle();
    }

    public void GameStart()
    {
        if (sceneLoad) return;
        sceneLoad = true;
        SceneManager.LoadSceneAsync(1);
    }

    public void GameRecord()
    {
        if (windowOn) return;
        windowOn = true;
        foreach (var page in manualPage)
        {
            page.SetActive(false);
        }
        recordWindow.SetActive(true);
        manualWindow.SetActive(false);
        noneSound = true;
        RecordSet();
    }

    public void GameManual()
    {
        if (windowOn) return;
        windowOn = true;
        recordWindow.SetActive(false);
        manualWindow.SetActive(true);
        manualPage[0].SetActive(true);
        noneSound = true;
    }

    public void ManualPage(int pageNum)
    {
        if (!manualWindow.activeSelf) return;
        
        foreach (var page in manualPage)
        {
            page.SetActive(false);
        }
        manualPage[pageNum].SetActive(true);
    }

    void RecordSet()
    {
        PlayerData.Instance.Set();
        Lv.text = $"{PlayerData.Instance.Lv}";
        //Debug.Log(PlayerData.Instance.Exp / (float)(500 * (1 + (PlayerData.Instance.Lv - 1) * 0.5f)));
        ExpGauge.fillAmount = PlayerData.Instance.Exp / (float)(500 * (1 + (PlayerData.Instance.Lv - 1) * 0.5f));
        Exp.text = 
            $"{PlayerData.Instance.Exp} / " +
            $"{500 * (1 + (PlayerData.Instance.Lv - 1) * 0.5f)}";
        MostSurvive.text = 
            $"{PlayerData.Instance.mostSurviveMinutes}:" +
            $"{PlayerData.Instance.mostSurviveSeconds}." +
            $"{PlayerData.Instance.mostSurviveMilSeconds}";
        MostStationDestroyed.text = $"{PlayerData.Instance.MostStationDestroyed}";
        MostEnemyDestroyed.text = $"{PlayerData.Instance.MostEnemyDestroyed}";
        MostBlockCombined.text = $"{PlayerData.Instance.MostBlockCombined}";
        MostBlockDestroyed.text = $"{PlayerData.Instance.MostBlockDestroyed}";
    }

    public void WindowClose()
    {
        windowOn = false;
        foreach (var page in manualPage)
        {
            page.SetActive(false);
        }
        recordWindow.SetActive(false);
        manualWindow.SetActive(false);
        noneSound = false;
    }
}
