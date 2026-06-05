using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentKeys = 0;

    [SerializeField] private HUDManager hudManager;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

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








}