using System.Collections;
using UnityEngine;

public class EnemyRanged : EnemyAI
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float preferredDistance = 8f;

    private bool isShooting;
    private EnemyHealth enemyHealth;

    protected override void Awake()
    {
        base.Awake();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    protected override void OnPlayerDetected()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Mantiene distancia preferida
        if (distance < preferredDistance)
        {
            Vector3 dirAway = (transform.position - player.position).normalized;
            agent.SetDestination(transform.position + dirAway * 2f);
        }
        else if (distance > preferredDistance + 2f)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }

        // Apunta y dispara
        Vector3 lookDirection = (player.position - transform.position);
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        if (!isShooting)
            StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        if (projectilePrefab != null && firePoint != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            direction.y = 0f;

            GameObject projectile = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation(direction)
            );

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = direction * projectileSpeed;

            Destroy(projectile, 4f);
        }

        yield return new WaitForSeconds(fireCooldown);
        isShooting = false;
    }
}