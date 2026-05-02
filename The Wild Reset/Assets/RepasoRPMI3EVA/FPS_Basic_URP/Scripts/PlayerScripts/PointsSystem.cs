using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsSystem : MonoBehaviour
{
    public int points;
    public int winPoints;
    public GameObject KeyPickup;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        KeyPickup = GameObject.Find("KeyPickup");
        KeyPickup.SetActive(false);
    }

    private void Update()
    {
        if (points >= winPoints) { KeyPickup.SetActive(true); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            points += 1;
            other.gameObject.SetActive(false);
        }
        if(other.gameObject.CompareTag("WinPick"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
