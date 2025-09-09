using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 0.2f;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Di chuy?n texture sang ph?i vô h?n
        float offsetX = Time.time * speed;
        rend.material.mainTextureOffset = new Vector2(offsetX, 0);
    }
}
