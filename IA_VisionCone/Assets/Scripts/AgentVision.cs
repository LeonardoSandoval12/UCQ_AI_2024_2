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
    public Transform Infiltrator;  // Transform del jugador que el agente est� buscando
    public Transform Agent;  // Transform de la cabeza del agente (punto de referencia para la visi�n)

    [Range(0f, 360f)]
    public float VisionAngle = 30f;  // Rango del �ngulo de visi�n del agente
    public float VisionDistance = 10f;  // Distancia m�xima de visi�n del agente

    private bool detected = false;  // Variable para almacenar si el jugador ha sido detectado

    //El giro del agente
    private float targetRotation = 0f;  // �ngulo de rotaci�n objetivo
    public float rotationSpeed = 45f;  // Velocidad de rotaci�n en grados por segundo

    private Coroutine rotationCoroutine;  // Referencia a la corrutina de rotaci�n

    private void Start()
    {
        // Inicia la rotaci�n del agente
        StartRotation();
    }

    private void Update()
    {
        // Reinicia la detecci�n
        detected = false;

        // Vector desde la cabeza del agente hasta el jugador
        Vector3 PlayerVector = Infiltrator.position - Agent.position;

        // Comprueba si el jugador est� dentro del �ngulo de visi�n
        if (Vector3.Angle(PlayerVector, Agent.forward) < VisionAngle * 0.5f)
        {
            // Comprueba si el jugador est� dentro de la distancia de visi�n
            if (PlayerVector.magnitude < VisionDistance)
            {
                // Si ambas condiciones se cumplen, el jugador es detectado
                detected = true;
            }
        }

        // Si el jugador es detectado, det�n el giro
        if (detected)
        {
            StopRotation();
        }
        else
        {
            // Si no es detectado, contin�a girando
            StartRotation();  // Asegura que la rotaci�n est� activa
        }
    }

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
        Gizmos.DrawLine(Agent.position, Agent.position + p1);
        Gizmos.DrawLine(Agent.position, Agent.position + p2);
        // Dibujar una l�nea central hacia adelante desde la cabeza
        Gizmos.DrawRay(Agent.position, Agent.forward * VisionDistance);
    }

    // M�todo para calcular un punto en el espacio 3D dado un �ngulo y una distancia
    Vector3 PointForAngle(float Angle, float Distance)
    {
        // Convertir el �ngulo a radianes
        float rad = Angle * Mathf.Deg2Rad;
        // Calcular y devolver el punto usando seno y coseno
        return Agent.TransformDirection(new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * Distance);
    }

    // M�todo para iniciar la rotaci�n del agente hacia el �ngulo objetivo
    private void StartRotation()
    {
        if (rotationCoroutine == null)
        {
            // Calcula el �ngulo objetivo sumando 90 grados al �ngulo actual
            targetRotation = Agent.eulerAngles.y + 90f;
            rotationCoroutine = StartCoroutine(RotateToTarget());
        }
    }

    // M�todo para detener la rotaci�n del agente
    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }

    // Corrutina para rotar el agente hacia el �ngulo objetivo
    private IEnumerator RotateToTarget()
    {
        float startRotation = Agent.eulerAngles.y;
        float timer = 0f;

        while (timer < 2f)  // Rotar durante 2 segundos
        {
            float angle = Mathf.Lerp(startRotation, targetRotation, timer / 2f);
            Agent.rotation = Quaternion.Euler(0, angle, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // Asegura que el agente termine exactamente en el �ngulo objetivo
        Agent.rotation = Quaternion.Euler(0, targetRotation, 0);

        // Espera un breve momento antes de reiniciar la rotaci�n si no se ha detenido
        yield return new WaitForSeconds(3f);

        if (detected == false)
        {
            // Reinicia la rotaci�n si no se ha detectado al jugador
            StartRotation();
        }
        else
        {
            rotationCoroutine = null;
        }
    }
}
