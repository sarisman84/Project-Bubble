using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eliott & Gabriel
public class MouseLook : MonoBehaviour
{
    public enum RotationAxis
    {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxis axes = RotationAxis.MouseXandY;

    [HideInInspector]
    [SerializeField]
    GameObject camera;
    public float minimumVert = -75.0f;
    public float maximumVert = 75.0f;
    public float sensitivityHor = 9.0f, sensitivityVert = 9.0f;
    public float _rotationX = 0;

    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }

    void Update() 
    {
        if (!GameManager.ins.inspecting)
        {
            if (axes == RotationAxis.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
            }
            else if (axes == RotationAxis.MouseY)
            {
                _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

                float rotationY = transform.localEulerAngles.y;

                transform.localEulerAngles = new Vector3(transform.localRotation.x, rotationY, 0);
            }
            else
            {
                _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

                float delta = Input.GetAxis("Mouse X") * sensitivityHor;
                float rotationY = transform.localEulerAngles.y + delta;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, 0);
                camera.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
            }
        }
    }
}
