using UnityEngine;

public class Gold : MonoBehaviour
{
    public int value = 1; // m?i vàng +1
    public AudioClip pickupSfx;
    public float sfxVolume = 0.8f;
    public float lifeTime = 0.5f; // t?n t?i 0.5s r?i bi?n m?t

    void Start()
    {
        // ? C?ng vàng ngay khi spawn
        GoldManager.Instance.AddGold(value);

        // ? Phát âm thanh
        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        // ? Xóa prefab vàng sau lifeTime (ch? ?? hi?n th?)
        Destroy(gameObject, lifeTime);
    }
}
