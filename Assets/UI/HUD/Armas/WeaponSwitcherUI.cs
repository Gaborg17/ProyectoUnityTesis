using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class WeaponSwitcherUI : MonoBehaviour
{
    [Header("Referencias de Slots (UI)")]
    [SerializeField] private RectTransform primarySlotRect;   // Cuadro de la Espada
    [SerializeField] private RectTransform secondarySlotRect; // Cuadro de la Pistola

    [Header("Ajustes de Animación")]
    [SerializeField] private float animDuration = 0.35f;

    private Vector2 primaryPos;
    private Vector2 secondaryPos;
    private Vector3 primaryScale;
    private Vector3 secondaryScale;
    private bool isPrimaryActive = true;
    private bool isAnimating = false;

    private void Start()
    {
        primaryPos = primarySlotRect.anchoredPosition;
        secondaryPos = secondarySlotRect.anchoredPosition;
        primaryScale = primarySlotRect.localScale;
        secondaryScale = secondarySlotRect.localScale;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SwitchWeapon();
        }
    }

    public void SwitchWeapon()
    {
        if (isAnimating) return;
        isAnimating = true;

        isPrimaryActive = !isPrimaryActive;

        // Traer al frente en el Canvas al cuadro que pasa a ser el Principal
        if (isPrimaryActive)
        {
            primarySlotRect.SetAsLastSibling();
        }
        else
        {
            secondarySlotRect.SetAsLastSibling();
        }

        Vector2 targetPosForPrimary = isPrimaryActive ? primaryPos : secondaryPos;
        Vector3 targetScaleForPrimary = isPrimaryActive ? primaryScale : secondaryScale;

        Vector2 targetPosForSecondary = isPrimaryActive ? secondaryPos : primaryPos;
        Vector3 targetScaleForSecondary = isPrimaryActive ? secondaryScale : primaryScale;

        // ANIMACIÓN DOTWEEN
        primarySlotRect.DOAnchorPos(targetPosForPrimary, animDuration).SetEase(Ease.OutBack);
        primarySlotRect.DOScale(targetScaleForPrimary, animDuration).SetEase(Ease.OutBack);

        secondarySlotRect.DOAnchorPos(targetPosForSecondary, animDuration).SetEase(Ease.OutBack);
        secondarySlotRect.DOScale(targetScaleForSecondary, animDuration).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                isAnimating = false;
            });
    }
}