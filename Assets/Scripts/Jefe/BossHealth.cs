using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Salud")]
    [SerializeField] private float maxHealth = 500f;
    private float currentHealth;

    [Header("Spawn de enemigos")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnCooldownNormal = 5f;
    [SerializeField] private float spawnCooldownEnraged = 2.5f;

    private bool isEnraged = false;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        HUDManager.Instance?.ShowBossHealth("The Compiler", currentHealth, maxHealth);
        StartCoroutine(SpawnRoutine());
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        HUDManager.Instance?.UpdateBossHealth(currentHealth, maxHealth);

        // Flash rojo igual que los enemigos normales
        StartCoroutine(FlashRed());

        // Activar modo enraged al 50%
        if (!isEnraged && currentHealth <= maxHealth * 0.5f)
        {
            isEnraged = true;
            Debug.Log("Jefe enraged!");
        }

        if (currentHealth <= 0f)
            Die();
    }

    public void Die()
    {
        isDead = true;
        HUDManager.Instance?.HideBossHealth();
        StopAllCoroutines();

        // Acá Manu puede agregar lo de abrir la sala final
        GameManager.Instance.OnBossDefeated();

        Destroy(gameObject, 0.5f);
    }

    // ── Spawn de enemigos ──────────────────────────────────

    private IEnumerator SpawnRoutine()
    {
        while (!isDead)
        {
            float cooldown = isEnraged ? spawnCooldownEnraged : spawnCooldownNormal;
            yield return new WaitForSeconds(cooldown);

            if (!isDead)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // Elegir prefab y punto de spawn al azar
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    // ── Flash de daño ──────────────────────────────────────

    private Renderer bossRenderer;
    private Color originalColor;

    private void Awake()
    {
        bossRenderer = GetComponentInChildren<Renderer>();
        if (bossRenderer != null)
            originalColor = bossRenderer.material.color;
    }

    private IEnumerator FlashRed()
    {
        if (bossRenderer == null) yield break;
        bossRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        bossRenderer.material.color = originalColor;
    }
}