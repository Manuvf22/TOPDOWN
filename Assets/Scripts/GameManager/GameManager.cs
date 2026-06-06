using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentKeys = 0;

    [SerializeField] private HUDManager hudManager;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    [SerializeField] private Barrier exitBarrier; // barrera que bloquea la sala final

    private void Awake()
    {
        Instance = this;
    }

    public void AddKey()
    {
        currentKeys++;
        hudManager?.UpdateKeys(currentKeys);
    }

    public int GetKeyCount() => currentKeys;

    public void UseKeys(int amount)
    {
        currentKeys = Mathf.Max(0, currentKeys - amount);
        hudManager?.UpdateKeys(currentKeys);
    }

    [SerializeField] private GameObject crosshair;

    public void OnPlayerWon()
    {
        victoryPanel?.SetActive(true);
        crosshair?.SetActive(false);
        Time.timeScale = 0f;
    }

    public void OnPlayerDied()
    {
        defeatPanel?.SetActive(true);
        crosshair?.SetActive(false);
        Time.timeScale = 0f;
    }

    public void OnBossDefeated()
    {
        // Abrir la salida a la sala final
        if (exitBarrier != null)
            exitBarrier.gameObject.SetActive(false);

        Debug.Log("Jefe derrotado, salida abierta!");
    }

}