using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLoadScene : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
