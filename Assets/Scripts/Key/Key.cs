using UnityEngine;

public class Key : MonoBehaviour, ICollectible
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnCollect();
    }

    public void OnCollect()
    {
        GameManager.Instance.AddKey();
        Destroy(gameObject);
    }
}