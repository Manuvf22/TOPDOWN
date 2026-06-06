using UnityEngine;
using System.Collections;

public class KeyIndicator3D : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite emptySlot;
    [SerializeField] private Sprite filledSlot;

    [Header("Configuración")]
    [SerializeField] private float slotSpacing = 0.6f;
    [SerializeField] private float floatAmplitude = 0.08f;
    [SerializeField] private float floatSpeed = 1.5f;

    private SpriteRenderer[] slots;
    private int filledCount = 0;
    private int totalSlots = 0;
    private Vector3 basePosition;

    public void Initialize(int keyCount)
    {
        totalSlots = keyCount;
        slots = new SpriteRenderer[keyCount];
        basePosition = transform.position;

        float totalWidth = slotSpacing * (keyCount - 1);
        float startX = -totalWidth / 2f;

        for (int i = 0; i < keyCount; i++)
        {
            GameObject slot = new GameObject($"KeySlot_{i}");
            slot.transform.SetParent(transform);
            slot.transform.localPosition = new Vector3(startX + i * slotSpacing, 0f, 0f);

            SpriteRenderer sr = slot.AddComponent<SpriteRenderer>();
            sr.sprite = emptySlot;
            sr.sortingOrder = 10;

            // Billboard para que mire a la cámara
            slot.AddComponent<BillboardFacing>();

            slots[i] = sr;
        }

        StartCoroutine(FloatRoutine());
    }

    public void FillNext()
    {
        if (filledCount >= totalSlots) return;
        slots[filledCount].sprite = filledSlot;
        filledCount++;
    }

    public bool AllFilled() => filledCount >= totalSlots;

    public IEnumerator OpenEffect()
    {
        // Flash verde
        for (int f = 0; f < 4; f++)
        {
            foreach (var sr in slots)
                if (sr != null) sr.color = Color.green;
            yield return new WaitForSeconds(0.08f);
            foreach (var sr in slots)
                if (sr != null) sr.color = Color.white;
            yield return new WaitForSeconds(0.08f);
        }

        // Fade out
        float elapsed = 0f;
        float duration = 0.4f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            foreach (var sr in slots)
            {
                if (sr == null) continue;
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator FloatRoutine()
    {
        while (true)
        {
            float y = basePosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(basePosition.x, y, basePosition.z);
            yield return null;
        }
    }
}