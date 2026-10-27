using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuPrincipal : MonoBehaviour
{
    // Función para el botón Jugar (carga el nivel de juego)
    public void JugarJuego()
    {
        // Reemplaza "Nivel1" por el nombre exacto de tu escena de juego
        SceneManager.LoadScene("Nivel1");
    }

    // Función para el botón Salir (cierra la aplicación)
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}