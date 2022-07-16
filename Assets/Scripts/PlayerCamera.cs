using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player settings")]
    public float mouseSensitivity = 4f;
    public float keySensitivity = 2f;
    public Light cameraLight;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        lookaround();
        flashlight();
    }

    void lookaround()
    {
        Quaternion xAxis, yAxis, zAxis, sumAxes;
        // Create multiple rotations for each axis
        xAxis = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSensitivity, Vector3.up);
        yAxis = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * mouseSensitivity, Vector3.right);
        if (Input.GetKey(KeyCode.E) != Input.GetKey(KeyCode.Q))
            zAxis = Quaternion.AngleAxis(Input.GetKey(KeyCode.Q) ? keySensitivity : -keySensitivity, Vector3.forward);
        else
            zAxis = Quaternion.AngleAxis(0f, Vector3.forward);
        // Consolidate all changes and add to object in one go
        sumAxes = xAxis * yAxis * zAxis;
        transform.rotation *= sumAxes;
    }

    void flashlight()
    {
        if (Input.GetKeyDown(KeyCode.X))
            cameraLight.enabled = !cameraLight.enabled;
    }
}
