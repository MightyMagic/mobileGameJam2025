using UnityEngine;

public class BowTurret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float rotationSpeed = 10f; // Добавлена скорость поворота

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

        // --- ЛОГИКА ПОВОРОТА К ЦЕЛИ ---
        Vector2 direction = (Vector2)baseTurret.currentTarget.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        AudioManager.Instance.PlaySound(SoundType.DefaultGun);

        if (projectile != null)
        {
            projectile.Seek(baseTurret.currentTarget);
        }
    }
}