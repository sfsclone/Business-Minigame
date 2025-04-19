using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BusinessManager : MonoBehaviour
{
    [System.Serializable]
    public class Business
    {
        public string name;
        public int cost;
        public int incomePerSecond;
        public Button button;
        public TMP_Text infoText;
        public bool isPurchased = false;
    }

    public List<Business> businesses = new List<Business>();
    private TapToEarn tapToEarn;

    void Start()
    {
        tapToEarn = FindAnyObjectByType<TapToEarn>();

        for (int i = 0; i < businesses.Count; i++)
        {
            int index = i;
            var b = businesses[index];

            // Load state
            b.isPurchased = PlayerPrefs.GetInt($"Business_{index}_Purchased", 0) == 1;
            UpdateBusinessUI(b, index);

            b.button.onClick.AddListener(() => PurchaseBusiness(index));
        }

        GameManager.Instance.SetBusinesses(businesses);
    }


    void PurchaseBusiness(int index)
    {
        var b = businesses[index];
        if (b.isPurchased) return;

        if (GameManager.Instance.money >= b.cost)
        {
            GameManager.Instance.money -= b.cost;
            b.isPurchased = true;

            // Save purchase state
            PlayerPrefs.SetInt($"Business_{index}_Purchased", 1);
            PlayerPrefs.Save();

            UpdateBusinessUI(b, index);
            tapToEarn?.UpdateMoneyUI();
        }
    }

    void UpdateBusinessUI(Business b, int index)
    {
        if (b.isPurchased)
        {
            b.button.interactable = false;
            b.infoText.text = $"{b.name} (Owned) - ${b.incomePerSecond}/sec";
        }
        else
        {
            b.button.interactable = true;
            b.infoText.text = $"{b.name} - Buy for ${b.cost}";
        }
    }

    IEnumerator GenerateIncome()
    {
        while (true)
        {
            foreach (var b in businesses)
            {
                if (b.isPurchased)
                {
                    GameManager.Instance.money += b.incomePerSecond;
                }
            }

            tapToEarn?.UpdateMoneyUI();
            yield return new WaitForSeconds(1f);
        }
    }
}
