//Este c�digo est� basado en el video de youtube proporcionado por el profesor
//https://youtu.be/lV47ED8h61k?si=6m012cxUMIkJvd5z

//Se us� la p�gina de global explorer
//https://explorer.globe.engineer/

// Importar las librer�as necesarias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVision : MonoBehaviour
{
    // Transform del jugador que el agente est� buscando
    public Transform Player;
    // Transform de la cabeza del agente (el punto de referencia para la visi�n)
    public Transform Head;

    // Rango del �ngulo de visi�n del agente (de 0 a 360 grados)
    [Range(0f, 360f)]
    public float VisionAngle = 30f;
    // Distancia m�xima de visi�n del agente
    public float VisionDistance = 10f;

    // Variable para almacenar si el jugador ha sido detectado
    bool detected = false;

    // M�todo que se llama en cada frame
    private void Update()
    {
        // Reiniciar la detecci�n
        detected = false;
        // Vector desde la cabeza del agente hasta el jugador
        Vector3 PlayerVector = Player.position - Head.position;

        // Comprobar si el jugador est� dentro del �ngulo de visi�n
        if (Vector3.Angle(PlayerVector, Head.forward) < VisionAngle * 0.5f)
        {
            // Comprobar si el jugador est� dentro de la distancia de visi�n
            if (PlayerVector.magnitude < VisionDistance)
            {
                // Si ambas condiciones se cumplen, el jugador es detectado
                detected = true;
            }
        }
    }

    // M�todo para dibujar en la escena (�til para depuraci�n)
    private void OnDrawGizmos()
    {
        // Si el �ngulo de visi�n es menor o igual a 0, no hacer nada
        if (VisionAngle <= 0f) return;

        // Calcular la mitad del �ngulo de visi�n
        float HalfVisionAngle = VisionAngle * 0.5f;

        // Calcular los puntos en los extremos del �ngulo de visi�n
        Vector3 p1 = PointForAngle(HalfVisionAngle, VisionDistance);
        Vector3 p2 = PointForAngle(-HalfVisionAngle, VisionDistance);

        // Cambiar el color del Gizmo seg�n si el jugador ha sido detectado o no
        Gizmos.color = detected ? Color.yellow : Color.blue;

        // Dibujar las l�neas del cono de visi�n
        Gizmos.DrawLine(Head.position, Head.position + p1);
        Gizmos.DrawLine(Head.position, Head.position + p2);
        // Dibujar una l�nea central hacia adelante desde la cabeza
        Gizmos.DrawRay(Head.position, Head.forward * VisionDistance);
    }

    // M�todo para calcular un punto en el espacio 3D dado un �ngulo y una distancia
    Vector3 PointForAngle(float Angle, float Distance)
    {
        // Convertir el �ngulo a radianes
        float rad = Angle * Mathf.Deg2Rad;
        // Calcular y devolver el punto usando seno y coseno
        return Head.TransformDirection(new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * Distance);
    }
}
