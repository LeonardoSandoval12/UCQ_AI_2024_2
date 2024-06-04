using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVision : MonoBehaviour
{
    public Transform Player;
    public Transform Head;

    [Range(0f, 360f)]
    public float VisionAngle = 30f;
    public float VisionDistance = 10f;

    bool detected = false;

    private void Update()
    {
        detected = false;
        Vector3 PlayerVector = Player.position - Head.position;

        // Comprobar si el jugador est� dentro del �ngulo de visi�n
        if (Vector3.Angle(PlayerVector, Head.forward) < VisionAngle * 0.5f)
        {
            // Comprobar si el jugador est� dentro de la distancia de visi�n
            if (PlayerVector.magnitude < VisionDistance)
            {
                detected = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (VisionAngle <= 0f) return;

        float HalfVisionAngle = VisionAngle * 0.5f;

        Vector3 p1 = PointForAngle(HalfVisionAngle, VisionDistance);
        Vector3 p2 = PointForAngle(-HalfVisionAngle, VisionDistance);

        Gizmos.color = detected ? Color.yellow : Color.blue;

        // Dibujar el cono de visi�n
        Gizmos.DrawLine(Head.position, Head.position + p1);
        Gizmos.DrawLine(Head.position, Head.position + p2);
        Gizmos.DrawRay(Head.position, Head.forward * VisionDistance);
    }

    Vector3 PointForAngle(float Angle, float Distance)
    {
        // Calcular el punto en el espacio 3D para el �ngulo dado
        float rad = Angle * Mathf.Deg2Rad;
        return Head.TransformDirection(new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * Distance);
    }
}