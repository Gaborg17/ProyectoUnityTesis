using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueUIController : MonoBehaviour
{
    [System.Serializable]
    public struct FactionStyle
    {
        public CharacterData.Faction faction;
        public Sprite tagSprite;      // Pestaña personalizada (verde, roja, azul, amarilla, etc.)
        public Sprite watermarkLogo;  // Logo de fondo (timón, saco de dinero, casas, etc.)
    }

    [Header("Animación de Panel (Entrada / Salida)")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("Referencias de UI")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image nameTagImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image watermarkImage;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private RectTransform nextArrowButton;

    [Header("Ajustes Animación Nombre (Ola)")]
    [SerializeField] private float waveSpeed = 2.0f;
    [SerializeField] private float waveHeight = 3.0f;

    [Header("Lista de Estilos por Bando/Rol")]
    [SerializeField] private List<FactionStyle> factionStyles;

    [Header("Velocidad de Texto")]
    [SerializeField] private float letterDelay = 0.03f;

    private Tween arrowTween;
    private Coroutine nameWaveCoroutine;
    private bool isPanelOpen = false;
    private Vector2 arrowOriginalPos;

    private void Start()
    {
        if (nextArrowButton != null)
        {
            arrowOriginalPos = nextArrowButton.anchoredPosition;
            StartArrowLoopAnimation();
        }

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
    }

    private void StartArrowLoopAnimation()
    {
        if (nextArrowButton == null) return;

        nextArrowButton.DOKill();
        nextArrowButton.anchoredPosition = arrowOriginalPos;

        arrowTween = nextArrowButton.DOAnchorPosX(arrowOriginalPos.x + 8f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void ShowDialogue(CharacterData character, string message)
    {
        if (character == null) return;

        if (!isPanelOpen)
        {
            OpenPanel();
        }

        PulseNextArrow();

        // 1. Obtener el estilo dinámicamente según la facción del personaje
        FactionStyle currentStyle = GetStyleForFaction(character.faction);

        // 2. Retrato
        if (portraitImage != null && character.portrait != null)
        {
            portraitImage.sprite = character.portrait;
            portraitImage.enabled = true;

            portraitImage.transform.DOKill();
            portraitImage.transform.localScale = Vector3.one * 0.8f;
            portraitImage.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        // 3. Pestaña y Nombre
        if (nameTagImage != null && currentStyle.tagSprite != null)
            nameTagImage.sprite = currentStyle.tagSprite;

        if (nameText != null)
        {
            nameText.text = character.characterName;

            if (nameWaveCoroutine != null) StopCoroutine(nameWaveCoroutine);
            nameWaveCoroutine = StartCoroutine(AnimateNameWave());
        }

        // 4. Marca de Agua
        if (watermarkImage != null && currentStyle.watermarkLogo != null)
        {
            watermarkImage.sprite = currentStyle.watermarkLogo;

            watermarkImage.transform.localScale = Vector3.one * 0.7f;
            Color c = watermarkImage.color;
            c.a = 0f;
            watermarkImage.color = c;

            watermarkImage.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
            watermarkImage.DOFade(0.15f, 0.4f);
        }

        // 5. Animación del texto
        StopAllCoroutines();
        nameWaveCoroutine = StartCoroutine(AnimateNameWave());
        StartCoroutine(TypewriterRoutine(message));
    }

    private FactionStyle GetStyleForFaction(CharacterData.Faction faction)
    {
        foreach (var style in factionStyles)
        {
            if (style.faction == faction)
                return style;
        }

        // Si no se encuentra un estilo específico, retorna el primero disponible como fallback
        return factionStyles.Count > 0 ? factionStyles[0] : new FactionStyle();
    }

    public void OpenPanel()
    {
        isPanelOpen = true;

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.DOKill();
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.DOFade(1f, fadeDuration);
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }

        if (panelRect != null)
        {
            panelRect.DOKill();
            panelRect.localScale = Vector3.one * 0.85f;
            panelRect.DOScale(1f, fadeDuration).SetEase(Ease.OutBack);
        }
    }

    public void HideDialogue()
    {
        isPanelOpen = false;

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.DOKill();
            panelCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                panelCanvasGroup.interactable = false;
                panelCanvasGroup.blocksRaycasts = false;
            });
        }

        if (panelRect != null)
        {
            panelRect.DOKill();
            panelRect.DOScale(0.85f, fadeDuration).SetEase(Ease.InBack);
        }
    }

    private void PulseNextArrow()
    {
        if (nextArrowButton != null)
        {
            if (arrowTween != null) arrowTween.Kill();
            nextArrowButton.DOKill();

            nextArrowButton.anchoredPosition = arrowOriginalPos;
            nextArrowButton.localScale = Vector3.one;

            nextArrowButton.DOPunchScale(Vector3.one * 0.35f, 0.2f, 5, 0.5f).OnComplete(() =>
            {
                StartArrowLoopAnimation();
            });
        }
    }

    private IEnumerator AnimateNameWave()
    {
        nameText.ForceMeshUpdate();
        TMP_TextInfo textInfo = nameText.textInfo;

        while (true)
        {
            nameText.ForceMeshUpdate();
            textInfo = nameText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float offsetY = Mathf.Sin(Time.time * waveSpeed + i * 1.5f) * waveHeight;

                vertices[vertexIndex + 0].y += offsetY;
                vertices[vertexIndex + 1].y += offsetY;
                vertices[vertexIndex + 2].y += offsetY;
                vertices[vertexIndex + 3].y += offsetY;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                nameText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    private IEnumerator TypewriterRoutine(string message)
    {
        dialogueText.text = message;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();

        int totalChars = message.Length;
        for (int i = 0; i <= totalChars; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    private void OnDestroy()
    {
        if (arrowTween != null) arrowTween.Kill();
    }
}