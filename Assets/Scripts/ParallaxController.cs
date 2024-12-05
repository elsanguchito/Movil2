using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float startPos, length;
    public float parallaxEffect; // The speed at which the background should move relative to the camera
    public GameObject cam; // Reference to the camera

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main.gameObject; // Asignar la cámara principal automáticamente
        }
    }

    void FixedUpdate()
    {
        // Calculate distance background move based on cam movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = move with cam || 1 = won't move || 0.5 = half
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // If background has reached the end of its length adjust its position for infinite scrolling
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}