using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaludJugador : MonoBehaviour
{
    public static SaludJugador Instance;

    [Header("Configuración de Vida")]
    public int vidaMaxima = 4;
    public int vidaActual;

    [Header("Inmunidad tras recibir daño")]
    public float tiempoInmunidad = 1f;
    private bool esInmune = false;

    [Header("Eventos de UI")]
    // Evento que notifica la vida actual para actualizar la interfaz (HUD)
    public UnityEvent<int> OnVidaCambiada;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        vidaActual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        // Notificar vida inicial
        OnVidaCambiada?.Invoke(vidaActual);
    }

    public void RecibirDaño(int cantidad)
    {
        if (esInmune) return;

        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        Debug.Log("Kalpa recibió daño. Gotas restantes: " + vidaActual);

        // Notificar cambio a la UI
        OnVidaCambiada?.Invoke(vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(RutinaInmunidad());
        }
    }

    // Método para curar o recolectar vida
    public void CurarVida(int cantidad)
    {
        if (vidaActual >= vidaMaxima) return;

        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;

        Debug.Log("Kalpa recuperó vida. Gotas actuales: " + vidaActual);

        // Notificar cambio a la UI
        OnVidaCambiada?.Invoke(vidaActual);
    }

    private IEnumerator RutinaInmunidad()
    {
        esInmune = true;
        
        // Efecto visual de parpadeo al recibir daño
        float tiempoPaso = 0.1f;
        for (float t = 0; t < tiempoInmunidad; t += tiempoPaso)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            yield return new WaitForSeconds(tiempoPaso);
        }

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        esInmune = false;
    }

    private void Morir()
    {
        Debug.Log("Kalpa ha muerto.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}