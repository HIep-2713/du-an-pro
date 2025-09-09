using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider manaSlider;          // Slider cho fill (cam/tr?ng)
    public Image backgroundImage;      // Background ?nh
    public Sprite normalBg;            // BG lúc bình th??ng
    public Sprite fullBg;              // BG khi ??y

    [Header("Mana Settings")]
    public int maxMana = 100;
    private int currentMana = 0;

    void Start()
    {
        if (manaSlider != null)
        {
            manaSlider.maxValue = maxMana;
            manaSlider.value = 0;
        }
        UpdateUI();
    }

    public void GainMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        UpdateUI();
    }

    public bool IsFull()
    {
        return currentMana >= maxMana;
    }

    public void ResetMana()
    {
        currentMana = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (manaSlider != null)
            manaSlider.value = currentMana;

        if (backgroundImage != null)
        {
            if (IsFull())
                backgroundImage.sprite = fullBg;
            else
                backgroundImage.sprite = normalBg;
        }
    }
}
