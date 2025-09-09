using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject menuPanel; // Panel ch?a menu d?c
    private bool isOpen = false;

    // G?i khi nh?n nút Gear
    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuPanel.SetActive(isOpen);
    }

    // G?i khi ch?n 1 nút trong menu
    public void CloseMenu()
    {
        isOpen = false;
        menuPanel.SetActive(false);
    }
}
