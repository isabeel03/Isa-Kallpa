using UnityEngine;
using Unity.Cinemachine; // Si usas Unity 6 / Cinemachine 3.x
// using Cinemachine; // Descomenta esta línea y borra la de arriba si usas Cinemachine v2 (versiones anteriores de Unity)

public class CinemachineAutoTarget : MonoBehaviour
{
    void Start()
    {
        // Buscar la cámara Cinemachine en este objeto
        var vcam = GetComponent<CinemachineCamera>(); // En versiones antiguas de Cinemachine usa: CinemachineVirtualCamera

        // Buscar al jugador activo (Kalipa)
        if (PlayerController.Instance != null && vcam != null)
        {
            vcam.Follow = PlayerController.Instance.transform;
            vcam.LookAt = PlayerController.Instance.transform;
        }
    }

    void Awake()
    {
        var vcam = GetComponent<CinemachineCamera>(); // o CinemachineVirtualCamera

        if (PlayerController.Instance != null && vcam != null)
        {
            // Asignar el objetivo
            vcam.Follow = PlayerController.Instance.transform;
            vcam.LookAt = PlayerController.Instance.transform;

            // Teletransportar la cámara inmediatamente para que no haga la transición desde (0,0)
            vcam.ForceCameraPosition(PlayerController.Instance.transform.position, Quaternion.identity);
        }
    }
}