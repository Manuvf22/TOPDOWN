using UnityEngine;


public class FloorButton : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private Transform spawnPoint;

    private bool isPressed = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 pos = transform.position;
        pos.y = -0.9f;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPressed) return;
        if (!other.CompareTag("Player")) return;

        Press();
    }

    private void Press()
    {
        isPressed = true;
        spriteRenderer.sprite = pressedSprite;

        if (powerUpPrefab != null && spawnPoint != null)
            Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);
    }
}