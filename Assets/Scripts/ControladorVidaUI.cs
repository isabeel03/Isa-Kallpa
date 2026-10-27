using UnityEngine;

public class ControladorVidaUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ControladorVidaUI : MonoBehaviour
{
    [Header("Imágenes de las Gotas de Vida")]
    [SerializeField] private Image[] gotasUI; // Arreglo con las 4 imágenes de la UI
    [SerializeField] private Sprite gotaLlena;
    [SerializeField] private Sprite gotaVacia; // Opcional (o desactivar la imagen)

    private void Start()
    {
        if (SaludJugador.Instance != null)
        {
            // Suscribirse al evento de cambio de vida
            SaludJugador.Instance.OnVidaCambiada.AddListener(ActualizarUI);
            ActualizarUI(SaludJugador.Instance.vidaActual);
        }
    }

    public void ActualizarUI(int vidaActual)
    {
        for (int i = 0; i < gotasUI.Length; i++)
        {
            if (i < vidaActual)
            {
                gotasUI[i].enabled = true;
                if (gotaLlena != null) gotasUI[i].sprite = gotaLlena;
            }
            else
            {
                if (gotaVacia != null)
                {
                    gotasUI[i].sprite = gotaVacia;
                }
                else
                {
                    gotasUI[i].enabled = false; // Desactiva la gota si no hay sprite vacío
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (SaludJugador.Instance != null)
        {
            SaludJugador.Instance.OnVidaCambiada.RemoveListener(ActualizarUI);
        }
    }
}