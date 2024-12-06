using UnityEngine;

public class CannonControler : MonoBehaviour
{
    public float shootForce = 10f;    // Fuerza inicial del disparo
    public float rollingSpeed = 2f;  // Velocidad de rodado en el suelo
    public float rotationSpeed = 100f;
    public float lifetime = 5f;      // Tiempo antes de destruir el objeto
    public Vector2 shootDirection = Vector2.right; // Dirección del disparo inicial
    public bool useGravity = true;  // Control de la gravedad (1 activada, 0 desactivada)

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 force = shootDirection.normalized * shootForce;
        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        // Alterna la gravedad según la propiedad `useGravity`
        rb.gravityScale = useGravity ? 1f : 0f;

        // Si el objeto toca el suelo, aplica velocidad constante horizontal
        if (rb.velocity.y <= 0.1f && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            Vector2 rollingVelocity = new Vector2(shootDirection.x * rollingSpeed, rb.velocity.y);
            rb.velocity = rollingVelocity;

            // Añade rotación basada en la velocidad horizontal
            float rotationAmount = -rollingVelocity.x * rotationSpeed;
            rb.MoveRotation(rb.rotation + rotationAmount * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController player = collision.gameObject.GetComponent<GameController>();
            player?.Die();
            Destroy(gameObject);
            return;
        }
        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Background"))
        {
            Destroy(gameObject);
        }
    }
}
