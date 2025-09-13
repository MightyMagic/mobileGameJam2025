using UnityEngine;

public class SawTurret : MonoBehaviour
{
    public float sawRotationSpeed = 200f; // Скорость вращения пилы вокруг турели
    public float bladeRotationSpeed = 500f; // Скорость вращения самого диска

    [Header("Saw Visuals")]
    public Transform sawBlade; // Ссылка на сам диск пилы

    void Update()
    {
        // Вращаем турель, чтобы пила двигалась по кругу
        transform.Rotate(0, 0, sawRotationSpeed * Time.deltaTime);

        // Вращаем сам диск пилы
        if (sawBlade != null)
        {
            sawBlade.Rotate(0, 0, bladeRotationSpeed * Time.deltaTime);
        }
    }

    // Визуализация в редакторе (необязательно, но полезно)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if (sawBlade != null)
        {
            Gizmos.DrawWireSphere(transform.position, Vector3.Distance(transform.position, sawBlade.position));
        }
    }
}