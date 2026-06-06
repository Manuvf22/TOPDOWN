using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private BossHealth boss;
    [SerializeField] private GameObject entranceBarrier;

    private bool triggered = false;

    private void Start()
    {
        // La barrera empieza desactivada
        if (entranceBarrier != null)
            entranceBarrier.SetActive(false);

        // El jefe empieza desactivado
        if (boss != null)
            boss.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (boss != null)
            boss.gameObject.SetActive(true);

        if (entranceBarrier != null)
            entranceBarrier.SetActive(true);

        gameObject.SetActive(false);
    }
}