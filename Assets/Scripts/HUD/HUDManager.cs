using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Integridad")]
    [SerializeField] private Image integrityFill;
    [SerializeField] private TextMeshProUGUI integrityLabel;

    [Header("Keys")]
    [SerializeField] private TextMeshProUGUI keysText;

    [Header("PowerUp")]
    [SerializeField] private GameObject powerUpPanel;
    [SerializeField] private TextMeshProUGUI powerUpName;
    [SerializeField] private Image powerUpBar;

    private Coroutine powerUpCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateIntegrity(float current, float max)
    {
        float ratio = current / max;

        if (integrityFill != null)
        {
            integrityFill.fillAmount = ratio;
            integrityFill.color = Color.Lerp(Color.red, Color.green, ratio);
        }

        if (integrityLabel != null)
        {
            int percent = Mathf.CeilToInt(ratio * 100);
            integrityLabel.text = $"Integridad: {percent}%";
        }
    }

    public void UpdateKeys(int keys)
    {
        if (keysText != null)
            keysText.text = $"x{keys}";
    }

    public void ShowPowerUp(string name, float duration)
    {
        if (powerUpPanel == null) return;
        powerUpPanel.SetActive(true);
        if (powerUpName != null) powerUpName.text = name;
        if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
        powerUpCoroutine = StartCoroutine(PowerUpBarRoutine(duration));
    }

    public void HidePowerUp()
    {
        powerUpPanel?.SetActive(false);
    }

    private System.Collections.IEnumerator PowerUpBarRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (powerUpBar != null)
                powerUpBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }
        HidePowerUp();
    }
}