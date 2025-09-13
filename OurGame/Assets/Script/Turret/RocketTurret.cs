using UnityEngine;

public class RocketTurret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;

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
        Rocket rocket = projectileGO.GetComponent<Rocket>();

        if (rocket != null)
        {
            rocket.Seek(baseTurret.currentTarget);
        }
    }
}