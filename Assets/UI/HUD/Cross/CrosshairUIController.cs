using UnityEngine;
using DG.Tweening;

public class CrosshairUIController : MonoBehaviour
{
    [Header("Referencias del Crosshair")]
    [SerializeField] private RectTransform outerReticle; // Círculo + 4 puntas (anima solo esto)
    [SerializeField] private RectTransform centerDot;    // Punto central (siempre quieto)

    [Header("Animación Automática (Chico a Grande)")]
    [SerializeField] private float minScale = 0.45f;      // Tamaño chico
    [SerializeField] private float maxScale = 1.35f;      // Tamaño grande
    [SerializeField] private float pulseDuration = 0.6f;  // Tiempo de chico a grande (o viceversa)
    [SerializeField] private Ease easeType = Ease.InOutSine;

    private Tween loopTween;

    // Guarda el último estado para detectar cambios en tiempo real dentro de Play
    private float lastMinScale;
    private float lastMaxScale;
    private float lastPulseDuration;
    private Ease lastEaseType;
    private RectTransform lastOuterReticle;
    private RectTransform lastCenterDot;

    private void OnEnable()
    {
        StartAutomaticLoop();
    }

    private void OnDisable()
    {
        StopAutomaticLoop();
    }

    // Se ejecuta automáticamente al editar valores en el Inspector
    private void OnValidate()
    {
        pulseDuration = Mathf.Max(0.01f, pulseDuration);

        if (Application.isPlaying && isActiveAndEnabled)
        {
            StartAutomaticLoop();
            CacheCurrentSettings();
        }
        else if (centerDot != null)
        {
            centerDot.localScale = Vector3.one;
        }
    }

    private void Update()
    {
        if (!isActiveAndEnabled) return;

        // Detecta si cambiaste Ease o algún ajuste mientras corre el juego
        if (HasSettingsChanged())
        {
            StartAutomaticLoop();
            CacheCurrentSettings();
        }
    }

    private bool HasSettingsChanged()
    {
        return !Mathf.Approximately(minScale, lastMinScale) ||
               !Mathf.Approximately(maxScale, lastMaxScale) ||
               !Mathf.Approximately(pulseDuration, lastPulseDuration) ||
               easeType != lastEaseType ||
               outerReticle != lastOuterReticle ||
               centerDot != lastCenterDot;
    }

    private void CacheCurrentSettings()
    {
        lastMinScale = minScale;
        lastMaxScale = maxScale;
        lastPulseDuration = pulseDuration;
        lastEaseType = easeType;
        lastOuterReticle = outerReticle;
        lastCenterDot = centerDot;
    }

    public void StartAutomaticLoop()
    {
        StopAutomaticLoop();
        CacheCurrentSettings();

        if (centerDot != null)
        {
            centerDot.localScale = Vector3.one;
        }

        if (outerReticle != null)
        {
            outerReticle.localScale = Vector3.one * minScale;

            loopTween = outerReticle.DOScale(maxScale, Mathf.Max(0.01f, pulseDuration))
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }
    }

    public void StopAutomaticLoop()
    {
        if (loopTween != null)
        {
            loopTween.Kill();
            loopTween = null;
        }

        if (centerDot != null)
        {
            centerDot.localScale = Vector3.one;
        }
    }

    private void OnDestroy()
    {
        StopAutomaticLoop();
    }
}