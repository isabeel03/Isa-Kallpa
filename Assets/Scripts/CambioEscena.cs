using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscena : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string nombreSiguienteEscena = "Nivel3"; 
    public string idPuntoEntradaDestino = "EntradaDesdeNivel2";

    [Header("Configuración de Transición")]
    public CanvasGroup panelFundido; 
    public float tiempoFundido = 0.5f; 

    private bool cambiando = false;

    private void Start()
    {
        // Respaldo por si la casilla está vacía en el Inspector
        if (panelFundido == null)
        {
            GameObject panelObj = GameObject.Find("PanelFundido");
            if (panelObj != null)
            {
                panelFundido = panelObj.GetComponent<CanvasGroup>();
            }
        }

        if (panelFundido != null)
        {
            panelFundido.alpha = 1f;
            StartCoroutine(AclararPantalla());
        }
        else
        {
            Debug.LogWarning("No se encontró el 'CanvasGroup' de PanelFundido en " + gameObject.name);
            cambiando = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !cambiando)
        {
            Debug.Log("Jugador detectado. Iniciando cambio a " + nombreSiguienteEscena);
            StartCoroutine(OscurecerYCargar());
        }
    }

    private IEnumerator OscurecerYCargar()
    {
        cambiando = true;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.puntoEntradaDestino = idPuntoEntradaDestino;
        }

        float tiempo = 0f;

        while (tiempo < tiempoFundido)
        {
            tiempo += Time.deltaTime;
            if (panelFundido != null)
            {
                panelFundido.alpha = Mathf.Clamp01(tiempo / tiempoFundido);
            }
            yield return null;
        }

        if (panelFundido != null) panelFundido.alpha = 1f;

        yield return new WaitForSeconds(0.05f);

        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    private IEnumerator AclararPantalla()
    {
        cambiando = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);

        float tiempo = tiempoFundido;

        while (tiempo > 0f)
        {
            tiempo -= Time.deltaTime;
            if (panelFundido != null)
            {
                panelFundido.alpha = Mathf.Clamp01(tiempo / tiempoFundido);
            }
            yield return null;
        }

        if (panelFundido != null) panelFundido.alpha = 0f;

        yield return new WaitForSeconds(0.2f);
        cambiando = false;
    }
}