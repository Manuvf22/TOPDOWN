using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyAI
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 1.5f;

    private bool isAttacking;
    private PlayerHealth playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    protected override void Update()
    {
        if (isAttacking) return;
        base.Update();
    }

    protected override void OnPlayerDetected()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
            StartCoroutine(Attack());
        else
            agent.SetDestination(player.position);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        agent.ResetPath();

        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (playerHealth != null && enemyHealth != null)
            playerHealth.TakeDamage(enemyHealth.DamageAmount);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;

        Debug.Log("EnemyMelee atacó al jugador por " + enemyHealth.DamageAmount + " de daño");
    }
}