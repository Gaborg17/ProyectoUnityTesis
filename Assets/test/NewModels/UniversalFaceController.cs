using UnityEngine;

[ExecuteAlways]
public class UniversalFaceController : MonoBehaviour
{
    [Header("Ojos (18 caras: 0-17)")]
    public Renderer eyeRenderer;
    [Range(0, 17)] public int eyeIndex = 0;

    [Header("Boca (12 caras: 0-11)")]
    public Renderer mouthRenderer;
    [Range(0, 11)] public int mouthIndex = 0;

    // Usa "_BaseMap" en URP o "_MainTex" en Built-in
    public string texturePropertyName = "_BaseMap";

    private int prevEye = -1;
    private int prevMouth = -1;
    private MaterialPropertyBlock eyeBlock;
    private MaterialPropertyBlock mouthBlock;
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponentInParent<Animator>();
        prevEye = -1;
        prevMouth = -1;
    }

    void LateUpdate()
    {
        // Mientras el Animator esté mezclando, se ignora el valor
        // y se deja la última cara. Al terminar la transición se aplica la nueva.
        if (Application.isPlaying && animator != null && IsAnyLayerInTransition())
            return;

        if (eyeIndex != prevEye)
        {
            Apply(eyeRenderer, ref eyeBlock, eyeIndex, 3, 6);
            prevEye = eyeIndex;
        }

        if (mouthIndex != prevMouth)
        {
            Apply(mouthRenderer, ref mouthBlock, mouthIndex, 4, 3);
            prevMouth = mouthIndex;
        }
    }

    bool IsAnyLayerInTransition()
    {
        for (int i = 0; i < animator.layerCount; i++)
            if (animator.IsInTransition(i)) return true;
        return false;
    }

    void Apply(Renderer rend, ref MaterialPropertyBlock block, int index, int col, int fil)
    {
        if (rend == null) return;
        if (block == null) block = new MaterialPropertyBlock();

        index = Mathf.Clamp(index, 0, col * fil - 1);

        float x = (index % col) * (1f / col);
        float y = -(index / col) * (1f / fil);

        rend.GetPropertyBlock(block);
        block.SetVector(texturePropertyName + "_ST", new Vector4(1f, 1f, x, y));
        rend.SetPropertyBlock(block);
    }
}