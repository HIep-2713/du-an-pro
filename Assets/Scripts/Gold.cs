using UnityEngine;

public class Gold : MonoBehaviour
{
    public int value = 1; // M?i vàng +1
    public AudioClip pickupSfx;
    public float sfxVolume = 0.8f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GoldManager.Instance.AddGold(value);

            // Phát âm thanh khi nh?t
            if (pickupSfx != null)
                AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

            Destroy(gameObject);
        }
    }
}
