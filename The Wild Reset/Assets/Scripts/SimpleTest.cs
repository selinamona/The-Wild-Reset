using UnityEngine;

public class SimpleTest : MonoBehaviour
{
    public float rayDistance = 3f;
    public bool hasKey = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Camera cam = GetComponentInChildren<Camera>();

        if (cam == null)
        {
            Debug.LogError("NO HAY CÁMARA");
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 2f);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.Log("Interactuando con: " + hit.collider.gameObject.name);

            GameObject objetoGolpeado = hit.collider.gameObject;

            // PRIMERO: Buscar scripts en el objeto golpeado Y en su padre
            KeyTest keyScript = objetoGolpeado.GetComponentInParent<KeyTest>();
            DoorTest doorScript = objetoGolpeado.GetComponentInParent<DoorTest>();

            // Intentar recoger llave
            if (keyScript != null)
            {
                keyScript.RecogerLlave();
                hasKey = true;
                Debug.Log("Ahora tienes la llave: " + hasKey);
                return;
            }

            // Intentar interactuar con puerta
            if (doorScript != null)
            {
                Debug.Log("Script de puerta encontrado. Bloqueada: " + doorScript.isLocked);

                if (doorScript.isLocked && hasKey)
                {
                    Debug.Log("Desbloqueando puerta...");
                    doorScript.UnlockDoor();
                    hasKey = false;
                }
                else if (!doorScript.isLocked)
                {
                    Debug.Log("Abriendo/cerrando puerta...");
                    doorScript.ToggleDoor();
                }
                else
                {
                    Debug.Log("La puerta está cerrada con llave. ¿Tienes llave?: " + hasKey);
                }
                return;
            }

            Debug.Log("El objeto golpeado no tiene KeyTest ni DoorTest");
        }
    }
}