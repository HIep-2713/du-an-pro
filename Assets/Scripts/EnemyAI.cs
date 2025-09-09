using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Step Move")]
    public float stepDelay = 0.3f;
    public float moveSpeed = 5f;
    public LayerMask obstacleMask;

    [Header("Knockback")]
    public float flySpeed = 6f;
    public AudioClip deadSfx;
    public float sfxVolume = 0.8f;

    [Header("Gold Drop")]
    public GameObject goldPrefab;
    [Range(0f, 1f)] public float dropChance = 0.5f;


    private Transform player;
    private Animator anim;
    private bool isMoving = false;
    private bool isDead = false;
    private Coroutine flyRoutine;
    private int currentState = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();

        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (!isDead)
        {
            if (!isMoving && player != null)
            {
                Vector3 diff = player.position - transform.position;

                // Dead zone ?? tránh ??i h??ng liên t?c khi g?n b?ng nhau
                float deadZone = 0.1f;
                Vector3 dir;

                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y) + deadZone)
                    dir = (diff.x > 0) ? Vector3.right : Vector3.left;
                else
                    dir = (diff.y > 0) ? Vector3.up : Vector3.down;

                Vector3 targetPos = transform.position + dir;

                if (CanMoveTo(targetPos))
                {
                    UpdateAnimation(dir);
                    isMoving = true;
                    yield return MoveStep(targetPos);
                    isMoving = false;
                }
            }
            yield return new WaitForSeconds(stepDelay);
        }
    }

    IEnumerator MoveStep(Vector3 targetPos)
    {
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }

    bool CanMoveTo(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.2f, obstacleMask);
        return hit == null;
    }

    void UpdateAnimation(Vector3 dir)
    {
        if (anim == null) return;

        int newState = currentState;
        if (dir == Vector3.down) newState = 0;
        else if (dir == Vector3.up) newState = 1;
        else if (dir == Vector3.right) newState = 2;
        else if (dir == Vector3.left) newState = 3;

        if (newState != currentState)
        {
            anim.SetInteger("State", newState);
            currentState = newState;
        }
    }

    // Enemy b? knockback (bình th??ng)
    public void Knockback(Vector2 direction, float force)
    {
        if (isDead) return;
        isDead = true;

        if (anim) anim.SetInteger("State", 4); // Dead animation

        if (deadSfx)
            AudioSource.PlayClipAtPoint(deadSfx, transform.position, sfxVolume);

        ScoreManager.Instance.AddScore(1);
        DropGold();

        if (flyRoutine != null) StopCoroutine(flyRoutine);
        flyRoutine = StartCoroutine(FlyOutRoutine(direction, force));
    }

    IEnumerator FlyOutRoutine(Vector2 dir, float force)
    {
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

    void DropGold()
    {
        if (goldPrefab != null && Random.value <= dropChance)
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
    }

    // === Special Kill: bi?n m?t ngay ===
    public void DieInstant()
    {
        if (isDead) return;
        isDead = true;

        if (deadSfx)
            AudioSource.PlayClipAtPoint(deadSfx, transform.position, sfxVolume);

        ScoreManager.Instance.AddScore(1);
        DropGold();

        Destroy(gameObject);
    }
}
