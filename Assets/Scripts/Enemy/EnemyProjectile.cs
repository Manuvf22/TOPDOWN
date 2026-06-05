using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Ignorar otros proyectiles enemigos
        if (other.CompareTag("EnemyProjectile")) return;
        // Ignorar a los propios enemigos
        if (other.CompareTag("Enemy")) return;

        // Buscar PlayerHealth en el objeto tocado O en su padre
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Si tocó cualquier otra cosa (paredes, suelo) se destruye
        Destroy(gameObject);
    }
}