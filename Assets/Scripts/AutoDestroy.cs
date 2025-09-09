using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 0.5f; // th?i gian t?n t?i (sau khi animation ch?y xong)

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
