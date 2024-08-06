using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int Lv;
    public int Exp;
    public int mostSurviveMinutes;
    public int mostSurviveSeconds;
    public int mostSurviveMilSeconds;
    public int MostStationDestroyed, MostEnemyDestroyed, 
        MostBlockCombined, MostBlockDestroyed;

    public void DeletData()
    {
        PlayerPrefs.DeleteAll();

        Lv = 1;
        Exp = 0;

        mostSurviveMinutes = 0;
        mostSurviveSeconds = 0;
        mostSurviveMilSeconds = 0;

        MostStationDestroyed = 0;
        MostEnemyDestroyed = 0;
        MostBlockDestroyed = 0;
        MostBlockCombined = 0;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Set();
    }

    public void Set()
    {
        Lv = PlayerPrefs.GetInt("PlayerLv", 1);
        Exp = PlayerPrefs.GetInt("PlayerExp", 0);

        mostSurviveMinutes = PlayerPrefs.GetInt("MostSurviveM", 0);
        mostSurviveSeconds = PlayerPrefs.GetInt("MostSurviveS", 0);
        mostSurviveMilSeconds = PlayerPrefs.GetInt("MostSurviveMS", 0);

        MostStationDestroyed = PlayerPrefs.GetInt("MostDestroyedS", 0);
        MostEnemyDestroyed = PlayerPrefs.GetInt("MostDestroyedE", 0);
        MostBlockCombined = PlayerPrefs.GetInt("MostCombinedB", 0);
        MostBlockDestroyed = PlayerPrefs.GetInt("MostDestroyedB", 0);
    }

    public int GameOver(int exp)
    {
        int lvUp = 0;

        Exp += exp;
        for (; 500 * (1 + (Lv - 1) * 0.5f) <= Exp;)
        {
            Exp -= (int)(500 * (1 + (Lv - 1) * 0.5f));
            Lv++;
            lvUp++;
        }

        PlayerPrefs.SetInt("PlayerLv", Lv);
        PlayerPrefs.SetInt("PlayerExp", Exp);

        return lvUp;
    }

    public bool GameOver(int mV, int sV, int msV)
    {
        bool renewRecord = false;

        if (mostSurviveMinutes < mV ||
            (mostSurviveMinutes == mV && mostSurviveSeconds > sV) ||
            (mostSurviveSeconds == sV && mostSurviveMilSeconds > msV))
        {
            mostSurviveMinutes = mV;
            mostSurviveSeconds = sV;
            mostSurviveMilSeconds = msV;

            PlayerPrefs.SetInt("MostSurviveM", mostSurviveMinutes);
            PlayerPrefs.SetInt("MostSurviveS", mostSurviveSeconds);
            PlayerPrefs.SetInt("MostSurviveMS", mostSurviveMilSeconds);

            renewRecord = true;
        }

        return renewRecord;
    }

    public bool GameOver(int recordType, int recordValue)
    {
        bool newRecord = false;

        switch(recordType)
        {
            case 0:
                if (PlayerPrefs.GetInt("MostDestroyedS", 0) < recordValue)
                {
                    PlayerPrefs.SetInt("MostDestroyedS", recordValue);
                    MostStationDestroyed = recordValue;
                    newRecord = true;
                }
                break;

            case 1:
                if (PlayerPrefs.GetInt("MostDestroyedE", 0) < recordValue)
                {
                    PlayerPrefs.SetInt("MostDestroyedE", recordValue);
                    MostEnemyDestroyed = recordValue;
                    newRecord = true;
                }
                break;

            case 2:
                if (PlayerPrefs.GetInt("MostCombinedB", 0) < recordValue)
                {
                    PlayerPrefs.SetInt("MostCombinedB", recordValue);
                    MostBlockCombined = recordValue;
                    newRecord = true;
                }
                break;

            case 3:
                if (PlayerPrefs.GetInt("MostDestroyedB", 0) < recordValue)
                {
                    PlayerPrefs.SetInt("MostDestroyedB", recordValue);
                    MostBlockDestroyed = recordValue;
                    newRecord = true;
                }
                break;
        }

        return newRecord;
    }
}
