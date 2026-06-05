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
}