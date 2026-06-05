using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private TextMeshProUGUI powerUpNameText;
    [SerializeField] private Image powerUpBar;

    private Coroutine powerUpCoroutine;

    private void Awake()
    {
        Instance = this;
        if (powerUpPanel != null)
            powerUpPanel.SetActive(false);
    }

    // ── Integridad ─────────────────────────────────────────

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

    // ── Keys ───────────────────────────────────────────────

    public void UpdateKeys(int keys)
    {
        if (keysText != null)
            keysText.text = $"x{keys}";
    }

    // ── PowerUp ────────────────────────────────────────────

    public void ShowPowerUp(string name, float duration)
    {
        if (powerUpPanel == null) return;
        if (powerUpNameText != null) powerUpNameText.text = name;
        if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
        powerUpCoroutine = StartCoroutine(PowerUpRoutine(duration));
    }

    public void HidePowerUp()
    {
        if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
        StartCoroutine(FadePanel(false));
    }

    private IEnumerator PowerUpRoutine(float duration)
    {
        // Fade de entrada
        yield return StartCoroutine(FadePanel(true));

        // Barra bajando como cooldown
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (powerUpBar != null)
                powerUpBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        // Fade de salida
        yield return StartCoroutine(FadePanel(false));
        powerUpCoroutine = null;
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
        // Agrega CanvasGroup si no existe
        CanvasGroup cg = powerUpPanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = powerUpPanel.AddComponent<CanvasGroup>();

        powerUpPanel.SetActive(true);

        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        float elapsed = 0f;
        float fadeDuration = 0.25f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = to;
        if (!fadeIn) powerUpPanel.SetActive(false);
    }
}