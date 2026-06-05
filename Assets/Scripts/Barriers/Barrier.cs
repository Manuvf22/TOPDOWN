using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private int keysRequired = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.GetKeyCount() >= keysRequired)
        {
            GameManager.Instance.UseKeys(keysRequired);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Necesitás {keysRequired} llaves. Tenés {GameManager.Instance.GetKeyCount()}.");
        }
    }
}