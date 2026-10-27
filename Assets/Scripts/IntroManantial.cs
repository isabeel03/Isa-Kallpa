using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using System.Collections; // Necesario para Coroutines

public class IntroManantial : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject panelDialogo; // Arrastra tu Panel de UI aquí
    public TextMeshProUGUI textoDialogo; // Arrastra tu Texto de UI aquí

    [Header("Configuración de Frases")]
    [TextArea(2, 4)]
    public string[] parrafosIntro = new string[]
    {
        "«Cuando la tierra se ahoga en veneno... el agua grita en silencio.»",
        "«Nacido de la última lágrima del manantial... levántate, Kallpa.»",
        "«Recoge la pureza que queda... y devuelve el latido a la tierra.»",
        "«Busca los núcleos perdidos... y restaura la vida.»"
    };

    public float duracionPorParrafo = 3f; // Tiempo en segundos por frase

    [Header("Referencias de Juego")]
    public GameObject kalpaObjeto; // Arrastra a tu jugador (desactivado) aquí

    void Start()
    {
        // 1. Al empezar, aseguramos que el panel esté oculto y el jugador desactivado
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (kalpaObjeto != null) kalpaObjeto.SetActive(false);

        // 2. Iniciamos la secuencia
        StartCoroutine(SecuenciaIntro());
    }

    IEnumerator SecuenciaIntro()
    {
        yield return new WaitForSeconds(1f);

        if (panelDialogo != null) panelDialogo.SetActive(true);

        // Recorremos los párrafos usando un índice para saber en cuál vamos
        for (int i = 0; i < parrafosIntro.Length; i++)
        {
            if (textoDialogo != null)
            {
                textoDialogo.text = parrafosIntro[i]; // Asigna la frase actual
            }

            // Esperar los 3 segundos de lectura de la frase actual
            yield return new WaitForSeconds(duracionPorParrafo);

            // Justo al terminar la 2da oración (índice 1 en programación):
            if (i == 1)
            {
                if (kalpaObjeto != null)
                {
                    kalpaObjeto.SetActive(true); // ¡El jugador se activa aquí!
                }
            }
        }

        // Ocultar el panel de diálogo tras mostrar las 4 frases
        if (panelDialogo != null) panelDialogo.SetActive(false);

        // Destruir el GameManager/Intro para limpiar la escena
        Destroy(gameObject);
    }
}