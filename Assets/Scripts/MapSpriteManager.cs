using UnityEngine;

public class MapSpriteManager : MonoBehaviour
{
    [Header("Water")]
    public Renderer waterRenderer;      // ??i t? SpriteRenderer sang Renderer (Quad)
    public Texture[] waterTextures;     // các texture water (wrap = Repeat)

    [Header("Cross")]
    public SpriteRenderer crossRenderer;
    public Sprite[] crossSprites;

    [Header("Lily 1 (3 cái)")]
    public SpriteRenderer[] lily1Renderers;
    public Sprite[] lily1Sprites;

    [Header("Lily 2 (9 cái)")]
    public SpriteRenderer[] lily2Renderers;
    public Sprite[] lily2Sprites;

    [Header("Lily 3 (1 cái)")]
    public SpriteRenderer[] lily3Renderers;
    public Sprite[] lily3Sprites;

    [Header("Lily 4 (1 cái)")]
    public SpriteRenderer[] lily4Renderers;
    public Sprite[] lily4Sprites;

    [Header("Special Effects")]
    public GameObject snowPrefab;
    private GameObject snowInstance;

    [Header("Special Effects")]
    public GameObject sakuraPrefab;
    private GameObject sakuraInstance;
    private ScrollingBackground scrollScript;

    void Start()
    {
        ChangeMap();
    }

    public void ChangeMap()
    {
        int index = Random.Range(0, 6); // 6 map

        // ??i texture Water (luôn wrap repeat)
        if (waterRenderer && waterTextures.Length > index)
            waterRenderer.material.mainTexture = waterTextures[index];

        // ??i sprite Cross
        if (crossRenderer && crossSprites.Length > index)
            crossRenderer.sprite = crossSprites[index];

        // Lily1
        if (lily1Sprites.Length > index)
        {
            Sprite chosen = lily1Sprites[index];
            foreach (var l in lily1Renderers)
                if (l) l.sprite = chosen;
        }

        // Lily2
        if (lily2Sprites.Length > index)
        {
            Sprite chosen = lily2Sprites[index];
            foreach (var l in lily2Renderers)
                if (l) l.sprite = chosen;
        }

        // Lily3
        if (lily3Sprites.Length > index)
        {
            Sprite chosen = lily3Sprites[index];
            foreach (var l in lily3Renderers)
                if (l) l.sprite = chosen;
        }
        if (lily4Sprites.Length > index)
        {
            Sprite chosen = lily4Sprites[index];
            foreach (var l in lily4Renderers)
                if (l) l.sprite = chosen;
        }
        // --- Logic map ---
        scrollScript = waterRenderer ? waterRenderer.GetComponent<ScrollingBackground>() : null;

        switch (index)
        {
            case 0: // Water
            case 1: // Water
            case 2: // Water
                EnableWater(true);
                EnableSnow(false);
                break;

            case 3: // Snow (winter)
                EnableWater(true);
                EnableSakura(true);
                break;

            case 4: // Snow + Water
                EnableWater(false);
                EnableSnow(true);
                break;

            case 5: // Bình th??ng (không water, không snow)
                EnableWater(false);
                EnableSnow(false);
                break;
        }
    }

    void EnableSnow(bool enable)
    {
        if (enable)
        {
            if (snowPrefab && snowInstance == null)
            {
                snowInstance = Instantiate(snowPrefab, Vector3.zero, Quaternion.identity);
            }
        }
        else
        {
            if (snowInstance) Destroy(snowInstance);
        }
    }

    void EnableSakura(bool enable)
    {
        if (enable)
        {
            if (sakuraPrefab && sakuraInstance == null)
            {
                sakuraInstance = Instantiate(sakuraPrefab, Vector3.zero, Quaternion.identity);
            }
        }
        else
        {
            if (sakuraInstance) Destroy(sakuraInstance);
        }
    }
    void EnableWater(bool enable)
    {
        if (scrollScript)
            scrollScript.enabled = enable;
    }
}
