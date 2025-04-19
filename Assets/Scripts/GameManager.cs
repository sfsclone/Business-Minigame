using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money;

    private List<BusinessManager.Business> businesses = new List<BusinessManager.Business>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        money = PlayerPrefs.GetInt("Money", 0);
        StartCoroutine(PassiveIncomeLoop());
        StartCoroutine(AutoSaveMoney());
    }

    public void SetBusinesses(List<BusinessManager.Business> bList)
    {
        businesses = bList;
    }

    IEnumerator PassiveIncomeLoop()
    {
        while (true)
        {
            foreach (var b in businesses)
            {
                if (b.isPurchased)
                {
                    money += b.incomePerSecond;
                }
            }

            FindAnyObjectByType<TapToEarn>()?.UpdateMoneyUI();
            yield return new WaitForSeconds(1f);
        }
    }
    public int GetTotalPassiveIncome()
    {
        int total = 0;
        foreach (var b in businesses)
        {
            if (b.isPurchased)
            {
                total += b.incomePerSecond;
            }
        }
        return total;
    }


    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    IEnumerator AutoSaveMoney()
    {
        while (true)
        {
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }
}
