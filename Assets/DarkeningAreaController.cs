using UnityEngine;
using System.Collections;

public class DarkenOnCollision : MonoBehaviour
{
    [SerializeField]
    private float darkenAmount = 0.5f; // Cuánto oscurecer (0 = original, 1 = completamente negro)

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si el objeto tiene un material con un renderer
        Renderer objectRenderer = other.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            DarkenObject(objectRenderer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Restaurar el color original al salir
        Renderer objectRenderer = other.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            RestoreObjectColor(objectRenderer);
        }
    }

    private void DarkenObject(Renderer renderer)
    {
        foreach (var material in renderer.materials)
        {
            Color color = material.color;
            color *= darkenAmount; // Oscurecer el color
            material.color = color;
        }
    }

    private void RestoreObjectColor(Renderer renderer)
    {
        foreach (var material in renderer.materials)
        {
            Color color = material.color;
            color /= darkenAmount; // Restaurar el color original
            material.color = color;
        }
    }
}
