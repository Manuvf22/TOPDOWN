using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        HUDManager.Instance?.UpdateIntegrity(currentHealth, maxHealth);
        Debug.Log("Jugador recibió daño. Vida restante: " + currentHealth + "/" + maxHealth);
        HUDManager.Instance?.TriggerDamageFlash(); // ← agregar esto
        if (currentHealth <= 0f)
            Die();
    }

    public void Die()
    {
        GameManager.Instance.OnPlayerDied();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        HUDManager.Instance?.UpdateIntegrity(currentHealth, maxHealth);
    }
}