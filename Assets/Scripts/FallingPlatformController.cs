using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Timing Settings")]
    public float fallDelay = 1f; // Tiempo antes de la ca�da.
    public float disappearDelay = 2f; // Tiempo antes de desaparecer.
    public float resetDelay = 5f; // Tiempo para reiniciar la plataforma.

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Guardar la posici�n y rotaci�n iniciales.
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            Invoke(nameof(Fall), fallDelay);
        }
    }

    private void Fall()
    {
        rb.isKinematic = false; // Permite que la plataforma sea afectada por la f�sica.
        rb.gravityScale = 1f; // Activa la gravedad.
        Invoke(nameof(Disappear), disappearDelay);
    }

    private void Disappear()
    {
        gameObject.SetActive(false); // Desactiva temporalmente la plataforma.
        Invoke(nameof(ResetPlatform), resetDelay);
    }

    private void ResetPlatform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.isKinematic = true;
        rb.gravityScale = 0f; // Aseg�rate de que no caiga hasta ser tocada nuevamente.
        isFalling = false;

        gameObject.SetActive(true);
    }
}
