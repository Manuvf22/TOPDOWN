using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int keysRequired = 3;

    [Header("Indicador de keys")]
    [SerializeField] private KeyIndicator3D keyIndicator;
    [SerializeField] private Vector3 indicatorOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Colisión")]
    [SerializeField] private Collider solidCollider;

    private int keysInserted = 0;
    private bool isOpen = false;

    private void Start()
    {
        if (keyIndicator != null)
        {
            keyIndicator.transform.position = transform.position + indicatorOffset;
            keyIndicator.Initialize(keysRequired);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        int playerKeys = GameManager.Instance.GetKeyCount();

        if (playerKeys >= 1)
        {
            GameManager.Instance.UseKeys(1);
            keysInserted++;
            keyIndicator?.FillNext();

            if (keysInserted >= keysRequired)
                StartCoroutine(OpenBarrier());
        }
        else
        {
            StartCoroutine(DeniedFlash());
        }
    }

    private IEnumerator OpenBarrier()
    {
        isOpen = true;

        // Desactivar colisión física inmediatamente
        if (solidCollider != null)
            solidCollider.enabled = false;

        // Efecto del indicador
        if (keyIndicator != null)
            yield return StartCoroutine(keyIndicator.OpenEffect());

        // Fade out de la barrera
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            float elapsed = 0f;
            float duration = 0.4f;
            Color original = rend.material.color;

            // Flash verde primero
            for (int f = 0; f < 4; f++)
            {
                rend.material.color = Color.green;
                yield return new WaitForSeconds(0.08f);
                rend.material.color = original;
                yield return new WaitForSeconds(0.08f);
            }

            // Fade
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Color c = original;
                c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
                rend.material.color = c;
                yield return null;
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator DeniedFlash()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) yield break;

        Color original = rend.material.color;
        rend.material.color = new Color(1f, 0.2f, 0.2f, 1f);
        yield return new WaitForSeconds(0.15f);
        rend.material.color = original;
    }
}