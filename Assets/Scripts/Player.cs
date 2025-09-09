using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Grid Settings")]
    public float tileSize = 1f;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    private Vector2 startTouchPos;

    [Header("Attack Effect")]
    public GameObject effectPrefab;
    public Vector2 effectOffset = Vector2.zero;

    [Header("Audio")]
    public AudioClip attackSfx;
    public AudioClip deadSfx;
    public AudioClip specialSfx;
    public float sfxVolume = 0.8f;

    [Header("Knockback")]
    public float flySpeed = 6f;
    public float knockbackForce = 8f;

    [Header("Mana System")]
    public ManaUI manaUI;
    public int manaPerHit = 20;

    [Header("Special Kill (Super)")]
    public GameObject shockwavePrefab;
    public GameObject superBulletPrefab; // 🔹 prefab SuperBullet (có Rigidbody2D + CircleCollider2D + SuperBullet script)
    public float specialRange = 8f;   // bán kính hit tất cả enemy

    private bool isDead = false;
    private Animator anim;
    private Coroutine knockbackRoutine;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif

        if (manaUI != null && manaUI.IsFull())
        {
            DoSpecialKill();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            startTouchPos = Input.mousePosition;
        else if (Input.GetMouseButtonUp(0))
            DetectSwipe(Input.mousePosition);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                startTouchPos = touch.position;
            else if (touch.phase == TouchPhase.Ended)
                DetectSwipe(touch.position);
        }
    }

    void DetectSwipe(Vector2 endPos)
    {
        Vector2 delta = endPos - startTouchPos;
        if (delta.magnitude >= swipeThreshold)
        {
            Vector2Int dir = GetCardinal(delta);

            UpdateFacingAnimation(dir);
            DoAttack(dir);
        }
    }

    Vector2Int GetCardinal(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? Vector2Int.right : Vector2Int.left;
        else
            return delta.y > 0 ? Vector2Int.up : Vector2Int.down;
    }

    void UpdateFacingAnimation(Vector2Int dir)
    {
        if (!anim) return;

        if (dir == Vector2Int.up) anim.SetTrigger("HitUp");
        else if (dir == Vector2Int.down) anim.SetTrigger("HitDown");
        else if (dir == Vector2Int.left) anim.SetTrigger("HitLeft");
        else if (dir == Vector2Int.right) anim.SetTrigger("HitRight");
    }

    void DoAttack(Vector2Int dir)
    {
        if (effectPrefab)
        {
            Vector3 spawn = transform.position + new Vector3(dir.x, dir.y, 0) * tileSize;
            spawn += (Vector3)effectOffset;

            GameObject fx = Instantiate(effectPrefab, spawn, Quaternion.identity);
            Animator fxAnim = fx.GetComponent<Animator>();
            if (fxAnim)
            {
                if (dir == Vector2Int.up) fxAnim.SetTrigger("AttackUp");
                else if (dir == Vector2Int.down) fxAnim.SetTrigger("AttackDown");
                else if (dir == Vector2Int.left) fxAnim.SetTrigger("AttackLeft");
                else if (dir == Vector2Int.right) fxAnim.SetTrigger("AttackRight");
            }
            Destroy(fx, 0.5f);
        }

        if (attackSfx)
            AudioSource.PlayClipAtPoint(attackSfx, transform.position, sfxVolume);

        if (anim)
            anim.SetTrigger("Punch");

        // ✅ Kiểm tra enemy phía trước
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir.x, dir.y), tileSize, LayerMask.GetMask("Enemy"));
        if (hit.collider != null)
        {
            if (manaUI != null)
                manaUI.GainMana(manaPerHit);
        }
    }

    // === Special Kill (Super) ===
    void DoSpecialKill()
    {
        if (anim) anim.SetTrigger("Special");
        if (specialSfx) AudioSource.PlayClipAtPoint(specialSfx, transform.position, sfxVolume);

        // reset mana
        manaUI.ResetMana();

        // gọi luôn super hit (đạn bay ra)
        DoSuperHit();
    }


    // Gọi từ animation event trong Super animation
    // Gọi từ animation event trong Super animation
    // Gọi từ animation event trong Super animation
    // Gọi từ animation event trong Super animation
    // Gọi từ animation event hoặc trực tiếp trong Special
    // Gọi từ animation event hoặc trực tiếp trong Special
    // Gọi trong DoSpecialKill()
    public void DoSuperHit()
    {
        // Bật animation super
        anim.Play("Special");
        StartCoroutine(SuperRoutine());
       
    }

    private IEnumerator SuperRoutine()
    {
        int waves = 3;
        float delay = 0.6f;
        float bulletLife = 0.5f;
        void KnockbackAllEnemies()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Hướng bay: từ player đẩy ra ngoài
                    Vector2 dir = (enemy.transform.position - transform.position).normalized;
                    rb.AddForce(dir * (knockbackForce * 2f), ForceMode2D.Impulse);
                }
            }
        }
        for (int i = 0; i < waves; i++)
        {
            // 🔹 Hất tung toàn bộ enemy ngay khi super bắt đầu đợt
            KnockbackAllEnemies();

            // 1. Spawn vòng tròn
            if (shockwavePrefab)
            {
                GameObject fx = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
                Destroy(fx, bulletLife);
            }

            // 2. Spawn đúng 8 viên đạn
            Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1,1).normalized, new Vector2(1,-1).normalized,
            new Vector2(-1,1).normalized, new Vector2(-1,-1).normalized
        };

            foreach (Vector2 dir in directions)
            {
                if (!superBulletPrefab) break;

                GameObject bullet = Instantiate(superBulletPrefab, transform.position, Quaternion.identity);
                var sb = bullet.GetComponent<SuperBullet>();
                if (sb != null)
                {
                    sb.knockbackForce = knockbackForce * 1.5f;
                    sb.speed = 8f;
                    sb.Fire(dir);
                }
                Destroy(bullet, bulletLife);
            }

            // 3. Đợi giữa các đợt
            yield return new WaitForSeconds(delay);
        }

        anim.Play("Idle");
        OnSuperEnd();
    }





















    // === Knockback khi player chết ===
    public void Knockback(Vector2 direction, float force)
    {
        if (knockbackRoutine != null) StopCoroutine(knockbackRoutine);
        knockbackRoutine = StartCoroutine(FlyOutRoutine(direction, force));
    }

    private IEnumerator FlyOutRoutine(Vector2 dir, float force)
    {
        if (isDead) yield break;
        isDead = true;

        if (anim) anim.SetTrigger("Dead");
        if (deadSfx) AudioSource.PlayClipAtPoint(deadSfx, transform.position, sfxVolume);

        Camera cam = Camera.main;
        Vector2 screenMin = cam.ViewportToWorldPoint(Vector2.zero);
        Vector2 screenMax = cam.ViewportToWorldPoint(Vector2.one);

        while (transform.position.x > screenMin.x - 1f &&
               transform.position.x < screenMax.x + 1f &&
               transform.position.y > screenMin.y - 1f &&
               transform.position.y < screenMax.y + 1f)
        {
            transform.position += (Vector3)(dir.normalized * force * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isDead)
        {
            Vector2 hitDir = (transform.position - collision.transform.position).normalized;
            Knockback(hitDir, knockbackForce);
        }
    }

    public void OnSuperEnd()
    {
        // 🔹 Xóa vòng tròn (shockwave)
        GameObject shockwave = GameObject.Find("Shockwave(Clone)");
        if (shockwave != null)
        {
            Destroy(shockwave);
        }

        // 🔹 Cho Player quay lại Idle
        if (anim)
        {
            anim.ResetTrigger("Special");
            anim.SetTrigger("Idle");
        }
    }
}
