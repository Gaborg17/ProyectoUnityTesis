using UnityEngine;

[ExecuteAlways]
public class EyeExpressionController : MonoBehaviour
{
    [Tooltip("Arrastra aquí el plano de los ojos de la vaca")]
    public Renderer eyeRenderer;

    [Tooltip("0=COOL, 1=OPEN, 2=CLOSED, 3=BLINK")]
    [Range(0, 3)]
    public int expression = 0;

    // Usa "_BaseMap" en URP o "_MainTex" en Built-in
    public string texturePropertyName = "_BaseMap";

    private int previousExpression = -1;
    private MaterialPropertyBlock propBlock;

    // Como el UV ya viene configurado en el cuadrante superior izquierdo desde Maya,
    // el estado base (COOL) requiere 0 de desplazamiento.
    private readonly Vector2[] offsets = new Vector2[]
    {
        new Vector2(0.0f,  0.0f),  // 0: COOL (Arriba - Izquierda)
        new Vector2(0.5f,  0.0f),  // 1: OPEN (Arriba - Derecha)
        new Vector2(0.0f, -0.5f),  // 2: CLOSED (Abajo - Izquierda)
        new Vector2(0.5f, -0.5f)   // 3: BLINK (Abajo - Derecha)
    };

    void Update()
    {
        if (expression != previousExpression)
        {
            UpdateExpression();
            previousExpression = expression;
        }
    }

    void UpdateExpression()
    {
        if (eyeRenderer == null) return;

        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();

        int index = Mathf.Clamp(expression, 0, offsets.Length - 1);

        eyeRenderer.GetPropertyBlock(propBlock);

        // Tiling es 1f, 1f para no encoger la UV que ya viene de Maya.
        // Solo inyectamos los offsets de X y Y.
        Vector4 tilingAndOffset = new Vector4(1f, 1f, offsets[index].x, offsets[index].y);

        propBlock.SetVector(texturePropertyName + "_ST", tilingAndOffset);
        eyeRenderer.SetPropertyBlock(propBlock);
    }
}