using UnityEngine;

public class FallingBlockController : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float detectionRange = 5f;
    public LayerMask groundLayer;
    public Vector2 boxSize;

    [Header("Movimiento")]
    public float fallSpeed = 5f;
    public float riseSpeed = 2f;
    public float groundCheckDistance = 0.5f;

    private Vector3 originalPosition;
    private bool isOnGround = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // Comprobar si el bloque está tocando el suelo
        isOnGround = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, groundCheckDistance, groundLayer);

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (isOnGround) return;

            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, riseSpeed * Time.deltaTime);
        }
    }
}
