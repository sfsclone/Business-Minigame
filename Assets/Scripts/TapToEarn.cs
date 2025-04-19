using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TapToEarn : MonoBehaviour
{
    public int moneyPerTap = 10;
    public int upgradeCost = 1000;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI incomeDisplayText;
    public Button tapButton;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;
    public TextMeshProUGUI passiveIncomeText;


    public GameObject rewardPrefab;
    public RectTransform canvasTransform;

    void Start()
    {
        // Load saved values
        GameManager.Instance.money = PlayerPrefs.GetInt("Money", 0);
        moneyPerTap = PlayerPrefs.GetInt("MoneyPerTap", 10);
        upgradeCost = PlayerPrefs.GetInt("UpgradeCost", 1000);

        // UI setup
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
        GameManager.Instance.money += moneyPerTap;
        UpdateMoneyUI();
        GameManager.Instance.SaveMoney();

        // Determine tap/touch position
        Vector2 screenPos = Input.touchCount > 0
            ? (Vector2)Input.GetTouch(0).position
            : (Vector2)Input.mousePosition;

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTransform, screenPos, null, out uiPos);

        GameObject clone = Instantiate(rewardPrefab, canvasTransform);
        clone.GetComponent<RectTransform>().anchoredPosition = uiPos;
    }

    void OnUpgradeTap()
    {
        if (GameManager.Instance.money >= upgradeCost)
        {
            GameManager.Instance.money -= upgradeCost;
            moneyPerTap *= 2;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 2f);

            // Save
            PlayerPrefs.SetInt("MoneyPerTap", moneyPerTap);
            PlayerPrefs.SetInt("UpgradeCost", upgradeCost);
            GameManager.Instance.SaveMoney();

            UpdateMoneyUI();
            UpdateUpgradeButtonText();
        }
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = $"Money: ${GameManager.Instance.money:N0}";
        if (incomeDisplayText != null)
            incomeDisplayText.text = $"Current: +${moneyPerTap} per tap";

        UpdatePassiveIncomeUI();
    }

    void UpdateUpgradeButtonText()
    {
        upgradeButtonText.text = $"Upgrade Income (${upgradeCost})";
        upgradeButton.interactable = GameManager.Instance.money >= upgradeCost;
    }
    public void UpdatePassiveIncomeUI()
    {
        int totalPassive = GameManager.Instance.GetTotalPassiveIncome();
        passiveIncomeText.text = $"Passive: +${totalPassive}/sec";
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            GameManager.Instance.SaveMoney();
            yield return new WaitForSeconds(5f);
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void OnApplicationQuit()
    {
        GameManager.Instance.SaveMoney();
    }
}
