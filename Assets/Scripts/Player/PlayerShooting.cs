using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float fireCooldown = 0.3f;

    private float nextFireTime;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Time.time < nextFireTime) return;

        Shoot();
        nextFireTime = Time.time + fireCooldown;
    }

    private void Shoot()
    {
        Vector3 direction = GetShootDirection();
        if (direction == Vector3.zero) return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, 3f);
    }

    private Vector3 GetShootDirection()
    {
        if (Mouse.current == null) return Vector3.zero;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, firePoint.position.y * Vector3.up);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 target = ray.GetPoint(enter);
            Vector3 direction = target - firePoint.position;
            direction.y = 0f;
            return direction.normalized;
        }

        return Vector3.zero;
    }
}