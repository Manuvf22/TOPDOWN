using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        // Ignorar al jugador
        if (other.CompareTag("Player")) return;

        // Dañar enemigo normal
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth == null)
            enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Dañar al jefe
        BossHealth bossHealth = other.GetComponent<BossHealth>();
        if (bossHealth == null)
            bossHealth = other.GetComponentInParent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Destruir al tocar suelo
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // Ignorar triggers de zona (BossTrigger, GoalZone, etc)
        // sin destruir el proyectil
        if (other.CompareTag("Untagged") && other.isTrigger) return;
    }
}