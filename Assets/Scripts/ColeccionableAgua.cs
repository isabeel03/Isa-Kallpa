using UnityEngine;

public class ColeccionableAgua : MonoBehaviour
{
    [Header("Cantidad de vida que restaura")]
    public int cantidadCuracion = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobamos si el objeto que chocó con la gota es el jugador
        if (collision.CompareTag("Player") || collision.GetComponent<SaludJugador>() != null)
        {
            // Llamamos a la función de curación del jugador
            if (SaludJugador.Instance != null)
            {
                SaludJugador.Instance.CurarVida(cantidadCuracion);
            }

            // Destruimos la gota del mapa para que desaparezca al recogerla
            Destroy(gameObject);
        }
    }
}