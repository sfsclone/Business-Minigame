using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Button reset;

    public int money;

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
        money = PlayerPrefs.GetInt("Money");
        
    }

    


    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }
}
