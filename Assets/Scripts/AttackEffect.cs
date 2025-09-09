using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float knockbackForce = 8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // N?u dính Enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                Vector2 hitDir = (other.transform.position - transform.position).normalized;
                enemy.Knockback(hitDir, knockbackForce); // g?i Knockback có force
            }
        }

        // N?u dính Player (trong tr??ng h?p có trap, ho?c enemy ?ánh ng??c l?i)
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Vector2 hitDir = (other.transform.position - transform.position).normalized;
                player.Knockback(hitDir, knockbackForce); // g?i Knockback có force
            }
        }
    }
}
