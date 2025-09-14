using UnityEngine;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(TMP_Text))]
public class TextWaveEditor : MonoBehaviour
{
    [Header("Wave Settings")]
    public float amplitude = 10f;   // высота волны
    public float wavelength = 50f;  // длина волны
    public float offset = 0f;       // сдвиг фазы по X

    TMP_Text textMesh;

    void OnEnable()
    {
        textMesh = GetComponent<TMP_Text>();
        ApplyWave();
    }

    void OnValidate()
    {
        if (textMesh == null)
            textMesh = GetComponent<TMP_Text>();

        ApplyWave();
    }

    void ApplyWave()
    {
        if (textMesh == null) return;

        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;

        if (textInfo.characterCount == 0) return; // текста ещё нет

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            // Проверяем, что индекс в пределах массива
            if (materialIndex >= textInfo.meshInfo.Length) continue;
            var vertices = textInfo.meshInfo[materialIndex].vertices;
            if (vertices == null || vertices.Length == 0) continue;

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = vertices[vertexIndex + j];
                float wave = Mathf.Sin((orig.x / wavelength) + offset) * amplitude;
                vertices[vertexIndex + j] = orig + new Vector3(0, wave, 0);
            }
        }

        // обновляем геометрию
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            if (meshInfo.mesh == null) continue;

            meshInfo.mesh.vertices = meshInfo.vertices;
            textMesh.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
