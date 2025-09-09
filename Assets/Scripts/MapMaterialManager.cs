using UnityEngine;

public class MapSpriteMa : MonoBehaviour
{
    [Header("Renderer")]
    public SpriteRenderer mapRenderer;

    [Header("Textures")]
    public Texture2D[] waterTextures; // 4 cái
    public Texture2D snowTexture;     // 1 cái
    public Texture2D defaultTexture;  // 1 cái

    [Header("Materials")]
    public Material waterMat;
    public Material snowMat;
    public Material defaultMat;

    void Start()
    {
        ChangeMa();
    }

    public void ChangeMa()
    {
        int index = Random.Range(0, 6); // 0-5

        if (index < 4) // 4 map n??c
        {
            mapRenderer.material = waterMat;
            mapRenderer.material.mainTexture = waterTextures[index];
        }
        else if (index == 4) // map tuy?t
        {
            mapRenderer.material = snowMat;
            mapRenderer.material.mainTexture = snowTexture;
        }
        else // map bình th??ng
        {
            mapRenderer.material = defaultMat;
            mapRenderer.material.mainTexture = defaultTexture;
        }
    }
}
