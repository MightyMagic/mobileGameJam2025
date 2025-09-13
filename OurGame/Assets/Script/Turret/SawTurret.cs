using UnityEngine;

public class SawTurret : MonoBehaviour
{
    public float sawRotationSpeed = 200f; // �������� �������� ���� ������ ������
    public float bladeRotationSpeed = 500f; // �������� �������� ������ �����

    [Header("Saw Visuals")]
    public Transform sawBlade; // ������ �� ��� ���� ����

    void Update()
    {
        // ������� ������, ����� ���� ��������� �� �����
        transform.Rotate(0, 0, sawRotationSpeed * Time.deltaTime);

        // ������� ��� ���� ����
        if (sawBlade != null)
        {
            sawBlade.Rotate(0, 0, bladeRotationSpeed * Time.deltaTime);
        }
    }

    // ������������ � ��������� (�������������, �� �������)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if (sawBlade != null)
        {
            Gizmos.DrawWireSphere(transform.position, Vector3.Distance(transform.position, sawBlade.position));
        }
    }
}