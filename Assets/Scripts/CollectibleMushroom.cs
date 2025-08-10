using UnityEngine;

public class CollectibleMushroom : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("[Mushroom] Picked");
        GameManager.Instance.AddMushroom(value);
        Destroy(gameObject);
    }
}



