using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;

    public Transform target;
    public float distance = 5.0f;

    public float bufferup = 1f;
    public float bufferright = 0.75f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && !target.GetComponent<PlayerController>().sesameDied() && !target.GetComponent<PlayerController>().game_over)
        {
            distance -= .5f * Input.mouseScrollDelta.y;
            if (distance < 0)
            {
                distance = 0;
            }
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0, 1.5f, -3) + target.position;// + new Vector3(0.0f, bufferup, 0.0f);

            transform.rotation = rotation;
            transform.position = position;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
