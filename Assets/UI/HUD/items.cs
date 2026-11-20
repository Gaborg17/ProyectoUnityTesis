using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ResourceCounterRoll : MonoBehaviour
{
    [Header("Texto TMP del recurso")]
    public TMP_Text textoNumero;

    [Header("Formato del número")]
    [Tooltip("Ej. 6 si quieres 000000; pon 0 si no quieres ceros a la izquierda")]
    public int digitosMinimos = 6;

    [Header("Velocidad del conteo")]
    [Tooltip("Duración de cada subida de número")]
    public float duracionConteo = 0.35f;

    [Header("Prueba rápida en Play (Nuevo Input System)")]
    public bool probarConTecla = false;
    public Key teclaPrueba = Key.F1;
    public int cantidadPrueba = 25;

    private int valorActual = 0;
    private Coroutine rutinaConteo;

    void Start()
    {
        if (textoNumero == null)
            textoNumero = GetComponent<TMP_Text>();

        if (textoNumero != null && int.TryParse(textoNumero.text, out int inicial))
        {
            valorActual = inicial;
        }

        ActualizarTexto(valorActual);
    }

    void Update()
    {
        if (probarConTecla && Keyboard.current != null && Keyboard.current[teclaPrueba].wasPressedThisFrame)
        {
            AgregarRecurso(cantidadPrueba);
        }
    }

    public void AgregarRecurso(int cantidad)
    {
        SetValor(valorActual + cantidad);
    }

    public void SetValor(int nuevoValor)
    {
        nuevoValor = Mathf.Max(0, nuevoValor);

        if (rutinaConteo != null)
            StopCoroutine(rutinaConteo);

        rutinaConteo = StartCoroutine(AnimarNumero(valorActual, nuevoValor));
    }

    private IEnumerator AnimarNumero(int desde, int hasta)
    {
        float dur = Mathf.Max(0.01f, duracionConteo);
        float tiempo = 0f;

        while (tiempo < dur)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempo / dur);
            t = 1f - Mathf.Pow(1f - t, 3f);

            int mostrado = Mathf.RoundToInt(Mathf.Lerp(desde, hasta, t));
            ActualizarTexto(mostrado);

            yield return null;
        }

        valorActual = hasta;
        ActualizarTexto(valorActual);
        rutinaConteo = null;
    }

    private void ActualizarTexto(int numero)
    {
        if (textoNumero == null) return;

        if (digitosMinimos > 0)
            textoNumero.text = numero.ToString("D" + digitosMinimos);
        else
            textoNumero.text = numero.ToString();
    }
}