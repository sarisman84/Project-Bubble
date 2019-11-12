using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eliott & Gabriel - SimonEDIT
public class MouseLook : MonoBehaviour
{
    public enum RotationAxis
    {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxis axes = RotationAxis.MouseXandY;


    [SerializeField] GameObject fpsCamera = null;
    public float minimumVert = -75.0f;
    public float maximumVert = 75.0f;
    public float sensitivityHor = 90f, sensitivityVert = 90f;
    public float _rotationX = 0;

    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
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
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * Time.deltaTime;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor * Time.deltaTime;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, 0);
            fpsCamera.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        }
    }
}
