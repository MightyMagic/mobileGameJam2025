using UnityEngine;

public class MachineGunTurret : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 3f; // 3 пули в секунду
    public float rotationSpeed = 10f; // Скорость поворота

    private Turret baseTurret;
    private float fireCountdown = 0f;

    void Start()
    {
        baseTurret = GetComponent<Turret>();
    }

    void Update()
    {
        if (baseTurret.currentTarget == null)
        {
            return;
        }

        // Поворот к цели
        Vector2 direction = (Vector2)baseTurret.currentTarget.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Логика стрельбы
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = bulletGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(baseTurret.currentTarget);
        }
    }
}