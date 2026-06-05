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

    [Header("Efectos de daño")]
    [SerializeField] private Image damageFlash;
    [SerializeField] private Image corruptionFrame;
    [SerializeField] private float flashDuration = 0.15f;

    private Coroutine powerUpCoroutine;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        Instance = this;
        if (powerUpPanel != null)
            powerUpPanel.SetActive(false);

        // Inicializar efectos invisibles
        if (damageFlash != null)
            SetAlpha(damageFlash, 0f);
        if (corruptionFrame != null)
            SetAlpha(corruptionFrame, 0f);
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

        UpdateCorruptionFrame(ratio);
    }

    // ── Marco de corrupción ────────────────────────────────

    private void UpdateCorruptionFrame(float ratio)
    {
        if (corruptionFrame == null) return;

        // Empieza a aparecer desde 40% hacia abajo
        // 40% → alpha 0, 0% → alpha 1
        float threshold = 0.4f;

        if (ratio >= threshold)
        {
            SetAlpha(corruptionFrame, 0f);
        }
        else
        {
            // Mapear 0.4→0 a alpha 0→1
            float alpha = Mathf.InverseLerp(threshold, 0f, ratio);
            SetAlpha(corruptionFrame, alpha);
        }
    }

    // ── Flash de daño ──────────────────────────────────────

    public void TriggerDamageFlash()
    {
        if (damageFlash == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetAlpha(damageFlash, 0.5f);
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsed / flashDuration);
            SetAlpha(damageFlash, alpha);
            yield return null;
        }

        SetAlpha(damageFlash, 0f);
        flashCoroutine = null;
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
        yield return StartCoroutine(FadePanel(true));

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (powerUpBar != null)
                powerUpBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        yield return StartCoroutine(FadePanel(false));
        powerUpCoroutine = null;
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
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

    // ── Utilidad ───────────────────────────────────────────

    private void SetAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
}