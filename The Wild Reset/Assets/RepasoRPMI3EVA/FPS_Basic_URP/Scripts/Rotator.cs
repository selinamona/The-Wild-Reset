using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeedX * Time.deltaTime);
        transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeedZ * Time.deltaTime);
    }
}
