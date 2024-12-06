using UnityEngine;

public class KeyController : MonoBehaviour
{
    public bool isFollowing = false;
    public Transform player; // Referencia al transform del jugador
    public float followSpeed = 5f; // Velocidad de seguimiento

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFollowing = true; // Activar el seguimiento

        }
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            // Mover la llave hacia el jugador
            transform.position = Vector2.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }
}
