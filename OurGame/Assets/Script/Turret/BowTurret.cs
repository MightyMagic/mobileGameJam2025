using UnityEngine;

public class BowTurret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1f;

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

        if (projectile != null)
        {
            projectile.Seek(baseTurret.currentTarget);
        }
    }
}