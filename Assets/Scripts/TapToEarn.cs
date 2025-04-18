using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TapToEarn : MonoBehaviour
{
    public int currentMoney;
    public int moneyPerTap = 10;
    public int upgradeCost = 1000;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI incomeDisplayText;
    public Button tapButton;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;

    public GameObject rewardPrefab;
    public RectTransform canvasTransform;

    void Start()
    {
        // Load saved values
        currentMoney = PlayerPrefs.GetInt("Money", 0);
        moneyPerTap = PlayerPrefs.GetInt("MoneyPerTap", 10);
        upgradeCost = PlayerPrefs.GetInt("UpgradeCost", 1000);

        // UI updates
        UpdateMoneyUI();
        UpdateUpgradeButtonText();

        // Button events
        tapButton.onClick.AddListener(OnTapEarn);
        upgradeButton.onClick.AddListener(OnUpgradeTap);

        // Start auto-save
        StartCoroutine(AutoSave());
    }

    void OnTapEarn()
    {
        currentMoney += moneyPerTap;
        UpdateMoneyUI();

        // Save immediately
        PlayerPrefs.SetInt("Money", currentMoney);

        // Visual feedback
        Vector2 screenPos = Input.mousePosition;
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, screenPos, null, out uiPos);

        GameObject clone = Instantiate(rewardPrefab, canvasTransform);
        clone.GetComponent<RectTransform>().anchoredPosition = uiPos;
    }

    void OnUpgradeTap()
    {
        if (currentMoney >= upgradeCost)
        {
            currentMoney -= upgradeCost;
            moneyPerTap *= 2;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 2f);

            // Save new values
            PlayerPrefs.SetInt("Money", currentMoney);
            PlayerPrefs.SetInt("MoneyPerTap", moneyPerTap);
            PlayerPrefs.SetInt("UpgradeCost", upgradeCost);

            UpdateMoneyUI();
            UpdateUpgradeButtonText();
        }
    }

    void UpdateMoneyUI()
    {
        moneyText.text = $"Money: ${currentMoney:N0}";
        if (incomeDisplayText != null)
            incomeDisplayText.text = $"Current :+${moneyPerTap} per tap";
    }

    void UpdateUpgradeButtonText()
    {
        upgradeButtonText.text = $"Upgrade Income (${upgradeCost})";
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            PlayerPrefs.SetInt("Money", currentMoney);
            PlayerPrefs.Save();
            yield return new WaitForSeconds(5f);
        }
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();
    }
}
