using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    float xRotation = 0f;

    [Header("Чувствительность мыши")]
    public float sensMs = 300f;

    public Transform Player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        mouseX=Input.GetAxis("Mouse X") * sensMs * Time.deltaTime;
        mouseY=Input.GetAxis("Mouse Y") * sensMs * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation=Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Player.Rotate(Vector3.up * mouseX);
    }
 
}
