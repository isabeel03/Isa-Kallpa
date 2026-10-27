using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControladorUIVida : MonoBehaviour
{
    [Header("Iconos de Vida")]
    public Image[] gotasVida; // Las 4 imágenes de las gotas
    public Sprite gotaLlena;
    public Sprite gotaVacia;

    [Header("Efecto de Despertar")]
    public CanvasGroup canvasGroupVida; // Para manejar la transparencia de la UI
    public float retrasoAparicion = 2f;  // Espera de 2 segundos
    public float tiempoFundido = 0.5f;   // Transición suave al aparecer

    private void Awake()
    {
        // Si no asignaste un CanvasGroup manualmente, intenta conseguirlo del mismo objeto
        if (canvasGroupVida == null)
        {
            canvasGroupVida = GetComponent<CanvasGroup>();
            if (canvasGroupVida == null)
            {
                canvasGroupVida = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Iniciar completamente invisible
        canvasGroupVida.alpha = 0f;
    }

    private void Start()
    {
        // Iniciar la rutina de espera de 2 segundos
        StartCoroutine(RutinaAparecerVida());
    }

    private void Update()
    {
        // Mantener las gotas actualizadas en tiempo real con la vida de Kalipa
        ActualizarGotas();
    }

    private IEnumerator RutinaAparecerVida()
    {
        // 1. Esperar 2 segundos mientras Kalipa despierta
        yield return new WaitForSeconds(retrasoAparicion);

        // 2. Transición suave de transparencia (alpha de 0 a 1)
        float tiempo = 0f;
        while (tiempo < tiempoFundido)
        {
            tiempo += Time.deltaTime;
            canvasGroupVida.alpha = Mathf.Clamp01(tiempo / tiempoFundido);
            yield return null;
        }

        canvasGroupVida.alpha = 1f;
    }

    public void ActualizarGotas()
    {
        if (SaludJugador.Instance == null) return;

        int vidaActual = SaludJugador.Instance.vidaActual;

        for (int i = 0; i < gotasVida.Length; i++)
        {
            if (gotasVida[i] == null) continue;

            if (i < vidaActual)
            {
                if (gotaLlena != null) gotasVida[i].sprite = gotaLlena;
                gotasVida[i].enabled = true;
            }
            else
            {
                if (gotaVacia != null)
                {
                    gotasVida[i].sprite = gotaVacia;
                }
                else
                {
                    gotasVida[i].enabled = false; // Desactiva la imagen si no hay sprite vacío
                }
            }
        }
    }
}