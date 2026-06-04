using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Top Down Shooter/Power Up")]
public class PowerUpData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private PowerUpType type;
    [SerializeField] private float value = 1f;
    [SerializeField] private float duration;
    [SerializeField] private Sprite icon;
    [SerializeField] private Color color = Color.white;

    public string DisplayName => displayName;
    public PowerUpType Type => type;
    public float Value => value;
    public float Duration => duration;
    public Sprite Icon => icon;
    public Color Color => color;
}