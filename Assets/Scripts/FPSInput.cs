using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eliott & Gabriel
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpSpeed = 6.0f;
    private float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    [SerializeField] Camera FPSCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        RaycastHit hit;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetKeyDown("space"))
            {
                moveDirection.y = jumpSpeed;
            }
            if (Input.GetKey(KeyCode.LeftControl)) //crouch
            {
                controller.height = controller.height / 2;
                FPSCamera.transform.localPosition = new Vector3(0f, 0.4f, 0f);
                jumpSpeed = 4f;
                speed = 3f;
                moveDirection.y = -jumpSpeed * 5;
            }
            else
            {
                if (!Physics.Raycast(transform.position, Vector3.up, out hit, 2)) //Hindrar spelaren att stå upp om man crouchar under nogonting
                {
                    controller.height = 2.5f;
                    FPSCamera.transform.localPosition = new Vector3(0f, 1.25f, 0f);
                    jumpSpeed = 6f;
                    speed = 5f;
                }
                else
                    Debug.Log("You cant stand up right now");
            }
            // PRONE OM DET BEHOVS!
            /*if (Input.GetKey(KeyCode.Z))  
            {
                controller.height = 0.4f;
                FPSCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
                jumpSpeed = 0f;
                speed = 1f;
                moveDirection.y = -jumpSpeed * 5;
            }*/
        }
        moveDirection.y -= (gravity * Time.deltaTime);
        controller.Move(Vector3.ClampMagnitude(moveDirection, speed) * Time.deltaTime); //Clamped to disallow quicker diagonal movement than straight - Simon Voss
    }
}