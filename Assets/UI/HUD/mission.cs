using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // Para las animaciones de la notificación
using UnityEngine.InputSystem;

public class MarqueeMissionController : MonoBehaviour
{
    [Header("Referencias de UI Misión")]
    [SerializeField] private TMP_Text missionText;
    [SerializeField] private RectTransform textContainer; // Objeto hijo con RectMask2D

    [Header("Referencias de Notificación (Pestaña)")]
    [SerializeField] private Image badgeImage; // La pestaña roja (Image)
    [SerializeField] private RectTransform badgeTransform; // RectTransform de la pestaña para el pulso
    [SerializeField] private Color normalBadgeColor = new Color(0.8f, 0.1f, 0.1f); // Rojo base
    [SerializeField] private Color notifyBadgeColor = Color.yellow; // Color de alerta/flash

    [Header("Ajustes de Movimiento")]
    [Tooltip("Velocidad del texto en píxeles por segundo")]
    [SerializeField] private float scrollSpeed = 80f;

    [Tooltip("Tiempo de espera antes y después de pasar el texto")]
    [SerializeField] private float pauseDuration = 1.5f;

    private RectTransform textRect;
    private Tween colorTween;

    private void Awake()
    {
        if (missionText != null)
        {
            textRect = missionText.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        // Prueba: Presiona M para lanzar una misión con efecto de notificación
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            SetMissionText("Mission: Go to the next island and talk to the pirate captain!");
        }
    }

    // Método público para actualizar la misión y detonar la notificación
    public void SetMissionText(string newMission)
    {
        // 1. Animación de Notificación con DOTween
        TriggerNotificationAnim();

        // 2. Reiniciar marquesina de texto
        StopAllCoroutines();
        StartCoroutine(AnimateMarqueeRoutine(newMission));
    }

    private void TriggerNotificationAnim()
    {
        if (badgeImage != null)
        {
            // Detener animaciones previas para evitar conflictos
            if (colorTween != null) colorTween.Kill();

            // Flash de color: Cambia al color brillante y regresa suavemente al normal
            badgeImage.color = notifyBadgeColor;
            colorTween = badgeImage.DOColor(normalBadgeColor, 0.8f).SetEase(Ease.OutQuad);
        }

        if (badgeTransform != null)
        {
            // Efecto de pulso / brinquito en la pestañita
            badgeTransform.DOKill();
            badgeTransform.localScale = Vector3.one;
            badgeTransform.DOPunchScale(Vector3.one * 0.25f, 0.4f, 5, 0.5f);
        }
    }

    private IEnumerator AnimateMarqueeRoutine(string textToDisplay)
    {
        missionText.text = textToDisplay;
        missionText.ForceMeshUpdate();
        float textWidth = missionText.preferredWidth;
        float containerWidth = textContainer.rect.width;

        // Si el texto cabe sin problemas, se queda estático al centro
        if (textWidth <= containerWidth)
        {
            textRect.anchoredPosition = new Vector2(0, textRect.anchoredPosition.y);
            yield break;
        }

        float startX = (containerWidth / 2f) + (textWidth / 2f);
        float endX = -(containerWidth / 2f) - (textWidth / 2f);

        while (true)
        {
            textRect.anchoredPosition = new Vector2(startX, textRect.anchoredPosition.y);
            yield return new WaitForSeconds(pauseDuration);

            float currentX = startX;

            while (currentX > endX)
            {
                currentX -= scrollSpeed * Time.deltaTime;
                textRect.anchoredPosition = new Vector2(currentX, textRect.anchoredPosition.y);
                yield return null;
            }

            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private void OnDestroy()
    {
        if (colorTween != null) colorTween.Kill();
    }
}