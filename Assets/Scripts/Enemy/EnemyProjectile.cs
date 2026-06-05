using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("Proyectil enemigo impactó al jugador por " + damage + " de daño");
        }


        if (!other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}