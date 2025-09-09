using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("UI hi?n th? vàng")]
    public Text goldText;

    private int goldCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        goldCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = "Gold: " + goldCount;
    }

    public int GetGold()
    {
        return goldCount;
    }
}
