using System.Collections;
using UnityEngine;

public class EnemyDash : EnemyAI
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float chargeDuration = 0.8f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashRange = 10f;
    [SerializeField] private float stunDuration = 1f;

    private bool isDashing;
    private bool isCharging;
    private bool isStunned;
    private PlayerHealth playerHealth;

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    protected override void Update()
    {
        if (isDashing || isCharging || isStunned) return;
        base.Update();
    }

    protected override void OnPlayerDetected()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= dashRange)
            StartCoroutine(DashSequence());
        else
            agent.SetDestination(player.position);
    }

    private IEnumerator DashSequence()
    {
        // Carga antes de dashear
        isCharging = true;
        agent.ResetPath();

        Vector3 dashDirection = (player.position - transform.position).normalized;
        dashDirection.y = 0f;

        yield return new WaitForSeconds(chargeDuration);
        isCharging = false;

        // Dash
        isDashing = true;
        agent.enabled = false;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        agent.enabled = true;

        // Cooldown antes del siguiente dash
        isStunned = true;
        yield return new WaitForSeconds(dashCooldown);
        isStunned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDashing) return;
        if (!other.TryGetComponent(out PlayerHealth health)) return;

        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            health.TakeDamage(enemyHealth.DamageAmount);

        StartCoroutine(StunAfterHit());
    }

    private IEnumerator StunAfterHit()
    {
        isDashing = false;
        agent.enabled = true;
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}