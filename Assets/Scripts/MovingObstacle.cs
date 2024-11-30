using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    public float speed;
    public int speedMultiplier = 1;
    public float waitDuration = 0f;

    [Header("Configuraci�n de Puntos de Control")]
    public GameObject ways; // Objeto padre que contiene los puntos de control
    public Transform[] wayPoints;

    private Vector3 targetPos;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;
    private bool isWaiting = false;

    private void Awake()
    {
        // Inicializar el arreglo de puntos de control con los hijos del objeto "ways"
        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointCount = wayPoints.Length; // N�mero total de puntos
        pointIndex = 1; // Comenzar desde el segundo punto (�ndice 1)
        targetPos = wayPoints[pointIndex].transform.position;
    }

    private void Update()
    {
        // Si est� esperando, no se mueve
        if (isWaiting) return;

        // Moverse hacia la posici�n objetivo
        var step = speed * speedMultiplier * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        // Comprobar si el objeto ha alcanzado la posici�n objetivo
        if (transform.position == targetPos)
        {
            StartCoroutine(WaitBeforeNextPoint());
        }
    }

    private System.Collections.IEnumerator WaitBeforeNextPoint()
    {
        // Esperar antes de avanzar al siguiente punto
        isWaiting = true;
        yield return new WaitForSeconds(waitDuration);
        isWaiting = false;
        NextPoint();
    }

    private void NextPoint()
    {
        // Cambiar direcci�n si se alcanza el primer o �ltimo punto
        if (pointIndex == pointCount - 1) // �ltimo punto
        {
            direction = -1;
        }

        if (pointIndex == 0) // Primer punto
        {
            direction = 1;
        }

        // Actualizar el �ndice y la posici�n objetivo
        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;
    }
}
