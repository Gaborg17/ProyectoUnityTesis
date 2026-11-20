using System.Collections;
using UnityEngine;

public class MenuSlideTransition : MonoBehaviour
{
    [Header("Paneles (Contenedores de 1920x1080)")]
    public RectTransform hudPanel;      // UI_AllyTab_HUD
    public RectTransform allyTabPanel;  // UI_AllyTab_Custom

    [Header("Posiciones X (0 = visible, -1920 = oculto a la izquierda)")]
    public float visibleX = 0f;
    public float ocultoIzquierdaX = -1920f;

    [Header("Tiempos (en segundos)")]
    [Tooltip("Duración exacta de cada transición")]
    public float duracion = 0.35f;

    [Header("Intensidad de Rebote y Anticipación (Solo AllyTab)")]
    [Tooltip("Qué tanto rebota AllyTab al entrar a pantalla (ej. 1.2 a 2.0)")]
    public float fuerzaRebote = 1.6f;
    [Tooltip("Qué tanto toma impulso hacia atrás antes de esconderse AllyTab (ej. 1.2 a 2.2)")]
    public float fuerzaAnticipacion = 1.7f;

    private Coroutine animacionActual;

    void Start()
    {
        // Empieza en el HUD visible y AllyTab escondido a la izquierda
        if (hudPanel != null)
            hudPanel.anchoredPosition = new Vector2(visibleX, hudPanel.anchoredPosition.y);

        if (allyTabPanel != null)
            allyTabPanel.anchoredPosition = new Vector2(ocultoIzquierdaX, allyTabPanel.anchoredPosition.y);
    }

    // Botón rojo del HUD -> esconde HUD limpio y saca AllyTab con rebote
    public void AbrirAllyTab()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarCambio(
            hudEsEntrando: false,
            destinoHudX: ocultoIzquierdaX,
            allyEsEntrando: true,
            destinoTabX: visibleX
        ));
    }

    // Botón rojo del AllyTab -> esconde AllyTab con anticipación y saca HUD limpio
    public void CerrarAllyTab()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarCambio(
            hudEsEntrando: true,
            destinoHudX: visibleX,
            allyEsEntrando: false,
            destinoTabX: ocultoIzquierdaX
        ));
    }

    private IEnumerator AnimarCambio(bool hudEsEntrando, float destinoHudX, bool allyEsEntrando, float destinoTabX)
    {
        float dur = Mathf.Max(0.01f, duracion);
        float tiempo = 0f;

        Vector2 inicioHud = hudPanel != null ? hudPanel.anchoredPosition : Vector2.zero;
        Vector2 inicioTab = allyTabPanel != null ? allyTabPanel.anchoredPosition : Vector2.zero;

        float yHud = hudPanel != null ? hudPanel.anchoredPosition.y : 0f;
        float yTab = allyTabPanel != null ? allyTabPanel.anchoredPosition.y : 0f;

        Vector2 finHud = new Vector2(destinoHudX, yHud);
        Vector2 finTab = new Vector2(destinoTabX, yTab);

        while (tiempo < dur)
        {
            tiempo += Time.unscaledDeltaTime;
            float lineal = Mathf.Clamp01(tiempo / dur);

            if (hudPanel != null)
            {
                // El HUD siempre se desliza limpio sin anticipación ni rebote (tanto al entrar como al salir)
                float easeHud = EaseOutCubic(lineal);
                hudPanel.anchoredPosition = Vector2.LerpUnclamped(inicioHud, finHud, easeHud);
            }

            if (allyTabPanel != null)
            {
                // AllyTab: rebote al entrar, anticipación al esconderse
                float easeTab = allyEsEntrando
                    ? EaseOutBack(lineal, fuerzaRebote)
                    : EaseInBack(lineal, fuerzaAnticipacion);

                allyTabPanel.anchoredPosition = Vector2.LerpUnclamped(inicioTab, finTab, easeTab);
            }

            yield return null;
        }

        if (hudPanel != null) hudPanel.anchoredPosition = finHud;
        if (allyTabPanel != null) allyTabPanel.anchoredPosition = finTab;
        animacionActual = null;
    }

    // Deslizamiento limpio sin anticipación ni rebote
    private float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    // Rebote al llegar a su lugar
    private float EaseOutBack(float x, float s)
    {
        float t = x - 1f;
        return t * t * ((s + 1f) * t + s) + 1f;
    }

    // Anticipación al salir/cerrar
    private float EaseInBack(float x, float s)
    {
        return x * x * ((s + 1f) * x - s);
    }
}