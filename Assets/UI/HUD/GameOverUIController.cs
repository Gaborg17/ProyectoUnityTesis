using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class GameOverUIController : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private Image flashOverlayImage;      // El destello blanco del golpe
    [SerializeField] private Image redOverlayImage;        // Fondo rojo transparente
    [SerializeField] private Image brushImage;             // Brochazo negro
    [SerializeField] private TMP_Text gameOverText;         // Texto GAME OVER
    [SerializeField] private Image fullBlackScreen;        // Pantalla negra final

    [Header("Tiempos de Animación")]
    [SerializeField] private float flashDuration = 0.15f;           // Duración del destello de golpe
    [SerializeField] private float fadeRedDuration = 0.5f;
    [SerializeField] private float brushDuration = 0.4f;
    [SerializeField] private float textFadeInDuration = 0.6f;
    [SerializeField] private float textHoldDuration = 1.2f;         // Pausa de suspenso tras aparecer el texto
    [SerializeField] private float blackScreenFadeDuration = 1.0f;  // Fade a negro
    [SerializeField] private float blackScreenHoldDuration = 2.0f;  // Segundos retenido en negro antes de volver
    [SerializeField] private float returnToTransparentDuration = 0.8f; // Qué tan suave vuelve a transparente

    [Header("Ajustes Onda Tenebrosa (Meneo Continuo)")]
    [SerializeField] private float waveSpeed = 6.0f;
    [SerializeField] private float waveHeight = 3.5f;

    private bool isWaveActive = false;

    private void Start()
    {
        ResetUIState();
    }

    private void Update()
    {
        // Presionar K para probar el golpe y la secuencia de muerte
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        ResetUIState();

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }

        Sequence gameOverSeq = DOTween.Sequence();

        // 1. FLASH DE IMPACTO: Entra visible al instante y se desvanece rápido
        if (flashOverlayImage != null)
        {
            flashOverlayImage.gameObject.SetActive(true);
            Color fc = flashOverlayImage.color;
            fc.a = 1f;
            flashOverlayImage.color = fc;

            gameOverSeq.Append(flashOverlayImage.DOFade(0f, flashDuration).SetEase(Ease.OutQuad));
        }

        // 2. Fade In del overlay rojo
        if (panelCanvasGroup != null)
        {
            gameOverSeq.Join(panelCanvasGroup.DOFade(1f, fadeRedDuration));
        }

        // 3. Brochazo negro pintándose de izquierda a derecha
        if (brushImage != null)
        {
            gameOverSeq.Append(brushImage.DOFillAmount(1f, brushDuration).SetEase(Ease.OutQuad));
        }

        // 4. Texto apareciendo con Fade In mientras ya se menea
        gameOverSeq.OnComplete(() =>
        {
            if (gameOverText != null)
            {
                StartCoroutine(SpookyTextSequence());
            }
            else
            {
                StartCoroutine(BlackScreenHoldAndReset());
            }
        });
    }

    private IEnumerator SpookyTextSequence()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.alpha = 0f;

        // Iniciar meneo constante
        isWaveActive = true;
        StartCoroutine(SpookyWaveLoop());

        // Fade in suave de todo el texto
        yield return gameOverText.DOFade(1f, textFadeInDuration).WaitForCompletion();

        // Pausa de suspenso
        yield return new WaitForSeconds(textHoldDuration);

        // 5. Fundido a negro total y retorno a transparente
        yield return StartCoroutine(BlackScreenHoldAndReset());
    }

    private IEnumerator BlackScreenHoldAndReset()
    {
        if (fullBlackScreen != null)
        {
            fullBlackScreen.gameObject.SetActive(true);
            Color c = fullBlackScreen.color;
            c.a = 0f;
            fullBlackScreen.color = c;

            // 1. Subir a negro total
            yield return fullBlackScreen.DOFade(1f, Mathf.Max(0.01f, blackScreenFadeDuration))
                .SetUpdate(true)
                .WaitForCompletion();

            // 2. Limpiar elementos intermedios sin matar el negro
            isWaveActive = false;
            if (gameOverText != null)
            {
                gameOverText.DOKill();
                gameOverText.alpha = 0f;
                gameOverText.gameObject.SetActive(false);
            }
            if (brushImage != null)
            {
                brushImage.DOKill();
                brushImage.fillAmount = 0f;
            }
            if (flashOverlayImage != null)
            {
                flashOverlayImage.DOKill();
                flashOverlayImage.gameObject.SetActive(false);
            }
            if (redOverlayImage != null)
            {
                redOverlayImage.DOKill();
                Color rc = redOverlayImage.color;
                rc.a = 0f;
                redOverlayImage.color = rc;
            }

            // 3. Retener en negro exactamente los segundos configurados en el Inspector
            float hold = Mathf.Max(0f, blackScreenHoldDuration);
            if (hold > 0f)
            {
                yield return new WaitForSecondsRealtime(hold);
            }

            // 4. Regresar a transparente
            yield return fullBlackScreen.DOFade(0f, Mathf.Max(0.01f, returnToTransparentDuration))
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .WaitForCompletion();
        }

        // 5. Dejar todo como al principio
        ResetUIState();
    }

    private IEnumerator SpookyWaveLoop()
    {
        while (isWaveActive && gameOverText != null)
        {
            gameOverText.ForceMeshUpdate();
            TMP_TextInfo textInfo = gameOverText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float offsetY = Mathf.Sin(Time.time * waveSpeed + i * 1.2f) * waveHeight;

                vertices[vertexIndex + 0].y += offsetY;
                vertices[vertexIndex + 1].y += offsetY;
                vertices[vertexIndex + 2].y += offsetY;
                vertices[vertexIndex + 3].y += offsetY;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                gameOverText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    private void ResetUIState()
    {
        StopAllCoroutines();
        isWaveActive = false;

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.DOKill();
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }

        if (flashOverlayImage != null)
        {
            flashOverlayImage.DOKill();
            Color fc = flashOverlayImage.color;
            fc.a = 0f;
            flashOverlayImage.color = fc;
            flashOverlayImage.gameObject.SetActive(false);
        }

        if (brushImage != null)
        {
            brushImage.DOKill();
            brushImage.fillAmount = 0f;
        }

        if (gameOverText != null)
        {
            gameOverText.DOKill();
            gameOverText.alpha = 0f;
            gameOverText.gameObject.SetActive(false);
        }

        if (fullBlackScreen != null)
        {
            fullBlackScreen.DOKill();
            Color c = fullBlackScreen.color;
            c.a = 0f;
            fullBlackScreen.color = c;
            fullBlackScreen.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        isWaveActive = false;
        if (panelCanvasGroup != null) panelCanvasGroup.DOKill();
        if (flashOverlayImage != null) flashOverlayImage.DOKill();
        if (brushImage != null) brushImage.DOKill();
        if (gameOverText != null) gameOverText.DOKill();
        if (fullBlackScreen != null) fullBlackScreen.DOKill();
    }
}