using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseFireCooldown = 0.4f;
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private float moveSpeedMultiplier = 1f;
    private float fireCooldownMultiplier = 1f;

    public float MoveSpeed => baseMoveSpeed * moveSpeedMultiplier;
    public float FireCooldown => baseFireCooldown * fireCooldownMultiplier;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier = multiplier;
    }

    public void ResetMoveSpeedMultiplier()
    {
        moveSpeedMultiplier = 1f;
    }

    public void SetFireCooldownMultiplier(float multiplier)
    {
        fireCooldownMultiplier = multiplier;
    }

    public void ResetFireCooldownMultiplier()
    {
        fireCooldownMultiplier = 1f;
    }
}