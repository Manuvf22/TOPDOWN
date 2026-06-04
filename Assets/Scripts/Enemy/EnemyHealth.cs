using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float flashDuration = 0.1f;

    private float currentHealth;
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isFlashing;

    public float DamageAmount => damageAmount;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (!isFlashing)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0f)
            Die();
    }

    private IEnumerator FlashRed()
    {
        isFlashing = true;
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material.color = originalColor;
        isFlashing = false;
    }

    private void Die()
    {
        // Acá van las partículas glitch cuando las tengás
        Destroy(gameObject);
    }
}