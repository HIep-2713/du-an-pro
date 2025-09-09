using UnityEngine;

public class Gold : MonoBehaviour
{
    public int value = 1; // m?i v�ng +1
    public AudioClip pickupSfx;
    public float sfxVolume = 0.8f;
    public float lifeTime = 0.5f; // t?n t?i 0.5s r?i bi?n m?t

    void Start()
    {
        // ? C?ng v�ng ngay khi spawn
        GoldManager.Instance.AddGold(value);

        // ? Ph�t �m thanh
        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        // ? X�a prefab v�ng sau lifeTime (ch? ?? hi?n th?)
        Destroy(gameObject, lifeTime);
    }
}
