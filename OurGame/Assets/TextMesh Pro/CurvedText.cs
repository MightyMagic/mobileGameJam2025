using UnityEngine;
using TMPro;

public class CurvedText : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    public float curveAngle = 90.0f;
    public float curveFrequency = 1.0f;

    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        m_TextComponent.ForceMeshUpdate();

        TMP_MeshInfo[] cachedMeshInfo = m_TextComponent.textInfo.meshInfo;

        int characterCount = m_TextComponent.textInfo.characterCount;

        if (characterCount == 0)
        {
            return;
        }

        Vector3[] vertices = cachedMeshInfo[0].vertices;
        int vertexIndex = 0;

        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo charInfo = m_TextComponent.textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            vertexIndex = charInfo.vertexIndex;

            float x0 = (vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2;
            float y0 = (vertices[vertexIndex + 0].y + vertices[vertexIndex + 1].y) / 2;

            float xOffset = x0 * curveFrequency;
            float yOffset = Mathf.Sin(xOffset) * curveAngle;

            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j].y += yOffset;
            }
        }

        m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}