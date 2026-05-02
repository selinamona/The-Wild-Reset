using UnityEngine;

public class KeyTest : MonoBehaviour
{
    void Start()
    {
        // Asegurarse que tiene collider
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        Debug.Log("Llave lista para ser recogida en: " + transform.position);
    }

    public void RecogerLlave()
    {
        Debug.Log("¡Llave recogida!");
        Destroy(gameObject);
    }
}