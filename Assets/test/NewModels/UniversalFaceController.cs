using UnityEngine;

[ExecuteAlways]
public class UniversalFaceController : MonoBehaviour
{
    [Header("Ojos")]
    [Tooltip("Arrastra la malla de los ojos de ESTE personaje")]
    public Renderer eyeRenderer;
    public int eyeColumnas = 0;
    public int eyeFilas = 0;
    public float eyeIndex = 0f;

    [Header("Boca")]
    [Tooltip("Arrastra la malla de la boca de ESTE personaje")]
    public Renderer mouthRenderer;
    public int mouthColumnas = 0;
    public int mouthFilas = 0;
    public float mouthIndex = 0f;

    [Header("Material")]
    public string texturePropertyName = "_BaseMap";

    // Memoria para no actualizar si no hay cambios
    private int prevEyeIndex = -1;
    private int prevMouthIndex = -1;

    private MaterialPropertyBlock eyePropBlock;
    private MaterialPropertyBlock mouthPropBlock;

    void OnEnable()
    {
        // Limpiamos la memoria al darle Play
        prevEyeIndex = -1;
        prevMouthIndex = -1;
        eyePropBlock = null;
        mouthPropBlock = null;
    }

    void LateUpdate()
    {
        // Revisamos y actualizamos los Ojos
        int currentEye = Mathf.RoundToInt(eyeIndex);
        if (currentEye != prevEyeIndex)
        {
            UpdateFacePart(eyeRenderer, eyeColumnas, eyeFilas, currentEye, ref eyePropBlock);
            prevEyeIndex = currentEye;
        }

        // Revisamos y actualizamos la Boca
        int currentMouth = Mathf.RoundToInt(mouthIndex);
        if (currentMouth != prevMouthIndex)
        {
            UpdateFacePart(mouthRenderer, mouthColumnas, mouthFilas, currentMouth, ref mouthPropBlock);
            prevMouthIndex = currentMouth;
        }
    }

    // Función universal matemática que sirve para cualquier parte de la cara
    void UpdateFacePart(Renderer rend, int col, int fil, int index, ref MaterialPropertyBlock block)
    {
        if (rend == null || col <= 0 || fil <= 0) return;
        if (block == null) block = new MaterialPropertyBlock();

        int maxIndex = (col * fil) - 1;
        int safeIndex = Mathf.Clamp(index, 0, maxIndex);

        float stepX = 1f / col;
        float stepY = 1f / fil;

        float offsetX = (safeIndex % col) * stepX;
        float offsetY = -(safeIndex / col) * stepY;

        rend.GetPropertyBlock(block);
        block.SetVector(texturePropertyName + "_ST", new Vector4(1f, 1f, offsetX, offsetY));
        rend.SetPropertyBlock(block);
    }
}