using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject earnPanel;
    public GameObject businessPanel;

    public void ShowEarnPanel()
    {
        earnPanel.SetActive(true);
        businessPanel.SetActive(false);
    }

    public void ShowBusinessPanel()
    {
        earnPanel.SetActive(false);
        businessPanel.SetActive(true);
    }
}
