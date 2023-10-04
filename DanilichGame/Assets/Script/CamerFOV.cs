using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFOV : MonoBehaviour
{
    private float fieldOfView = 73.0f;
    void Start()
    {
        Camera.main.fieldOfView = fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetMouseButton(1)) {
            Camera.main.fieldOfView -= 10.0f;
        }else{
            Camera.main.fieldOfView += 10.0f;
        }
    }
}
