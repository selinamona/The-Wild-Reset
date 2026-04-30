using UnityEngine;
using System.Collections;

public class DoorTest : MonoBehaviour
{
    [Header("Configuración de Puerta")]
    public GameObject doorVisual;        // El modelo 3D de la puerta
    public float openAngle = 90f;        // Ángulo de apertura
    public float openSpeed = 2f;         // Velocidad de apertura
    public bool isLocked = true;         // Estado de bloqueo

    [Header("Eje de Rotación")]
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // Eje Y por defecto
    public bool rotateAroundParent = true; // Rotar alrededor del padre

    [Header("Debug")]
    public bool isOpen = false;

    private bool isAnimating = false;
    private Quaternion closedRotation;

    void Start()
    {
        if (doorVisual == null)
        {
            Debug.LogError("FALTA ASIGNAR doorVisual en: " + gameObject.name);
            // Intentar encontrar automáticamente
            doorVisual = GetComponentInChildren<MeshRenderer>()?.gameObject;
            if (doorVisual == null)
                doorVisual = transform.GetChild(0)?.gameObject;
        }

        if (doorVisual != null)
        {
            closedRotation = doorVisual.transform.localRotation;
            Debug.Log("Puerta configurada correctamente. Visual: " + doorVisual.name);

            // Asegurarse que el visual tiene collider
            if (doorVisual.GetComponent<Collider>() == null)
            {
                BoxCollider col = doorVisual.AddComponent<BoxCollider>();
                col.size = new Vector3(0.1f, 2f, 1f); // Tamaño típico de puerta
            }
        }

        Debug.Log("Puerta lista. Posición: " + transform.position + " | Bloqueada: " + isLocked);
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("¡Puerta desbloqueada!");
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        if (!isAnimating && doorVisual != null)
        {
            StopAllCoroutines();
            isOpen = !isOpen;
            StartCoroutine(RotateDoor());
        }
    }

    IEnumerator RotateDoor()
    {
        isAnimating = true;

        // Determinar qué objeto rotar
        Transform objectToRotate = rotateAroundParent ? transform : doorVisual.transform;

        // Calcular rotaciones
        Quaternion startRotation = objectToRotate.localRotation;
        Quaternion targetRotation;

        if (isOpen)
        {
            // Abrir - rotar en el eje especificado
            targetRotation = startRotation * Quaternion.Euler(rotationAxis * openAngle);
        }
        else
        {
            // Cerrar - volver a rotación original
            targetRotation = startRotation * Quaternion.Euler(rotationAxis * -openAngle);
        }

        Debug.Log("Rotando puerta. Desde: " + startRotation.eulerAngles + " Hasta: " + targetRotation.eulerAngles);

        float elapsed = 0;
        float duration = 1f / openSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Interpolación suave
            objectToRotate.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        objectToRotate.localRotation = targetRotation;
        isAnimating = false;

        Debug.Log(isOpen ? "Puerta ABIERTA" : "Puerta CERRADA");
    }

    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        if (doorVisual != null || transform.childCount > 0)
        {
            Transform visualTransform = doorVisual != null ? doorVisual.transform : transform.GetChild(0);

            Gizmos.color = Color.red;
            Vector3 worldAxis = transform.TransformDirection(rotationAxis);
            Gizmos.DrawRay(transform.position, worldAxis * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(visualTransform.position, visualTransform.localScale);
        }
    }
}