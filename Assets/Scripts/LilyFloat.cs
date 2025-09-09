using UnityEngine;

public class LilyFloat : MonoBehaviour
{
    public float floatStrength = 0.1f; // biên ?? n?i
    public float floatSpeed = 2f;      // t?c ?? n?i

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Lily ch? n?i lên xu?ng, không trôi theo dòng n??c
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = startPos + new Vector3(0, newY, 0);
    }
}
