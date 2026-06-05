using UnityEngine;

public class PlayerVisualRotation : MonoBehaviour
{
    [Tooltip("Velocidad de giro en grados por segundo")]
    [SerializeField] private float rotationSpeed = 180f;

    private PlayerController playerController;
    private bool isMoving = false;

    // Referencia al CharacterController para saber si se mueve
    private CharacterController characterController;
    private Vector3 lastPosition;

    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Detectar si el jugador se está moviendo comparando posición
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        isMoving = distanceMoved > 0.001f;
        lastPosition = transform.position;

        if (isMoving)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}