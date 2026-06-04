using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpData powerUpData;
    [SerializeField] private Renderer visualRenderer;

    private void Awake()
    {
        ApplyVisualConfiguration();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerPowerUpReceiver playerPowerUpReceiver))
            return;

        playerPowerUpReceiver.ApplyPowerUp(powerUpData);
        Destroy(gameObject);
    }

    private void ApplyVisualConfiguration()
    {
        if (powerUpData == null || visualRenderer == null)
            return;

        visualRenderer.material.color = powerUpData.Color;
    }
}