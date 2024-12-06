using UnityEngine;

public class DoorController : MonoBehaviour
{
    private CompositeCollider2D doorCollider;
    private void Start()
    {
        // Obtener el BoxCollider2D del objeto al que está asignado este script
        doorCollider = GetComponent<CompositeCollider2D>();

        if (doorCollider == null)
        {
            Debug.LogError("No se encontró un BoxCollider2D en la puerta.");
        }
    }

    private void Update()
    {
        KeyController key = FindObjectOfType<KeyController>();

        if (key != null && key.isFollowing && doorCollider != null)
        {
            doorCollider.isTrigger = true;
        }
        else if (doorCollider != null)
        {
            doorCollider.isTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KeyController key = FindObjectOfType<KeyController>();

            if (key != null && key.isFollowing)
            {
                Destroy(gameObject);
            }
        }
    }
}
