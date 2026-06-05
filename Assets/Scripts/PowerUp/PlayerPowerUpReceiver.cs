using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerPowerUpReceiver : MonoBehaviour
{
    private PlayerStats playerStats;
    private Coroutine speedBoostCoroutine;
    private Coroutine rapidFireCoroutine;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void ApplyPowerUp(PowerUpData powerUpData)
    {
        if (powerUpData == null) return;

        switch (powerUpData.Type)
        {
            case PowerUpType.Heal:
                // Buscar PlayerHealth en vez de PlayerStats
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                    playerHealth.Heal(powerUpData.Value);
                break;

            case PowerUpType.SpeedBoost:
                ApplySpeedBoost(powerUpData.Value, powerUpData.Duration);
                HUDManager.Instance?.ShowPowerUp(
                    powerUpData.DisplayName, powerUpData.Duration);
                break;

            case PowerUpType.RapidFire:
                ApplyRapidFire(powerUpData.Value, powerUpData.Duration);
                HUDManager.Instance?.ShowPowerUp(
                    powerUpData.DisplayName, powerUpData.Duration);
                break;

            case PowerUpType.DamageBoost:
                Debug.Log("Damage boost not implemented yet.");
                break;

            case PowerUpType.Shield:
                Debug.Log("Shield not implemented yet.");
                break;
        }
    }

    private void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);
        speedBoostCoroutine = StartCoroutine(
            ApplySpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator ApplySpeedBoostRoutine(float multiplier, float duration)
    {
        playerStats.SetMoveSpeedMultiplier(multiplier);
        yield return new WaitForSeconds(duration);
        playerStats.ResetMoveSpeedMultiplier();
        speedBoostCoroutine = null;
    }

    private void ApplyRapidFire(float multiplier, float duration)
    {
        if (rapidFireCoroutine != null)
            StopCoroutine(rapidFireCoroutine);
        rapidFireCoroutine = StartCoroutine(
            ApplyRapidFireRoutine(multiplier, duration));
    }

    private IEnumerator ApplyRapidFireRoutine(float multiplier, float duration)
    {
        playerStats.SetFireCooldownMultiplier(multiplier);
        yield return new WaitForSeconds(duration);
        playerStats.ResetFireCooldownMultiplier();
        rapidFireCoroutine = null;
    }
}