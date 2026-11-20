using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class BountyUIController : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private TMP_Text bountyText;            // El texto del número (ej. "2500$")
    [SerializeField] private RectTransform posterTransform;  // El RectTransform del cartel de WANTED

    [Header("Ajustes de Animación")]
    [SerializeField] private float countDuration = 0.5f;      // Duración del conteo rápido de números
    [SerializeField] private float punchScaleAmount = 0.2f;   // Escalado de impacto
    [SerializeField] private float punchOffsetY = 15f;        // Distancia del brinco vertical hacia arriba
    [SerializeField] private Color normalColor = Color.black; // Color original del texto
    [SerializeField] private Color flashColor = new Color(1f, 0.85f, 0f); // Dorado/Amarillo al subir

    [Header("Prueba")]
    [SerializeField] private int initialBounty = 2500;
    [SerializeField] private int testAddAmount = 500;         // Cuánto suma con la tecla B

    private int currentBounty = 0;
    private Tween countTween;
    private Tween colorTween;
    private Vector2 originalAnchoredPos;
    private Vector3 originalScale;

    private void Start()
    {
        if (posterTransform != null)
        {
            // Guardar posición y escala base exactas del Inspector
            originalAnchoredPos = posterTransform.anchoredPosition;
            originalScale = posterTransform.localScale;
        }

        currentBounty = initialBounty;
        UpdateTextDirect(currentBounty);
    }

    private void Update()
    {
        // Tecla B para probar subir el Bounty
        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            AddBounty(testAddAmount);
        }
    }

    /// <summary>
    /// Llama a este método desde la lógica del juego cuando el jugador robe algo.
    /// </summary>
    public void AddBounty(int amountToAdd)
    {
        int targetBounty = currentBounty + amountToAdd;
        AnimateBountyIncrease(currentBounty, targetBounty);
        currentBounty = targetBounty;
    }

    private void AnimateBountyIncrease(int startValue, int endValue)
    {
        // 1. REINICIAR Y RESETEAR TRANSFORMACIONES (Para evitar que se quede chueco o deformado)
        if (posterTransform != null)
        {
            posterTransform.DOKill();
            posterTransform.anchoredPosition = originalAnchoredPos;
            posterTransform.localScale = originalScale;
            posterTransform.localRotation = Quaternion.identity; // Asegura rotación en 0 siempre

            // Animación A: Golpecito/Brinco vertical (sube ligeramente y cae)
            posterTransform.DOPunchAnchorPos(new Vector2(0f, punchOffsetY), 0.35f, 8, 0.5f)
                .OnComplete(() => posterTransform.anchoredPosition = originalAnchoredPos);

            // Animación B: Pulsación de tamaño fluida (OutBack)
            posterTransform.DOPunchScale(Vector3.one * punchScaleAmount, 0.3f, 5, 0.5f)
                .OnComplete(() => posterTransform.localScale = originalScale);
        }

        // 2. CONTEO NUMÉRICO RÁPIDO Y FLUIDO
        if (countTween != null) countTween.Kill();

        countTween = DOVirtual.Float(startValue, endValue, countDuration, (val) =>
        {
            int tempValue = Mathf.RoundToInt(val);
            UpdateTextDirect(tempValue);
        }).SetEase(Ease.OutQuad);

        // 3. DESTELLO DE COLOR EN EL TEXTO DEL NÚMERO
        if (bountyText != null)
        {
            if (colorTween != null) colorTween.Kill();
            bountyText.color = flashColor;
            colorTween = bountyText.DOColor(normalColor, countDuration + 0.15f).SetEase(Ease.OutQuad);
        }
    }

    private void UpdateTextDirect(int value)
    {
        if (bountyText != null)
        {
            bountyText.text = $"{value}$";
        }
    }

    private void OnDestroy()
    {
        if (countTween != null) countTween.Kill();
        if (colorTween != null) colorTween.Kill();
        if (posterTransform != null) posterTransform.DOKill();
    }
}