using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eliott & Gabriel - Simon EDIT
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    private float speed = 5.0f;
    private float jumpSpeed = 6.0f;
    private float gravity = 20.0f;

    [SerializeField] float defaultMoveSpeed = 5f;
    [SerializeField] float defaultJumpSpeed = 6f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    [SerializeField] Camera FPSCamera = null;
    float headHeightDefault;
    float headHeightCrouch = 0.4f;
    float characterHeightDefault;

    public bool canMove = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        headHeightDefault = FPSCamera.transform.localPosition.y;
        characterHeightDefault = controller.height;
    }
    
    void Update()
    {
        RaycastHit hit;

        if (controller.isGrounded && canMove)
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
                controller.height = controller.height * 0.5f;
                FPSCamera.transform.localPosition = new Vector3(0f, headHeightCrouch, 0f);
                jumpSpeed = defaultJumpSpeed * 0.5f;
                speed = defaultMoveSpeed * 0.5f;
                moveDirection.y = -jumpSpeed * 5;
            }
            else
            {
                if (!Physics.Raycast(transform.position, Vector3.up, out hit, 2)) //Hindrar spelaren att stå upp om man crouchar under nogonting
                {
                    controller.height = characterHeightDefault;
                    FPSCamera.transform.localPosition = new Vector3(0f, headHeightDefault, 0f);
                    jumpSpeed = defaultJumpSpeed;
                    speed = defaultMoveSpeed;
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

        if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = defaultMoveSpeed * 1.5f;
            }
        else
            {
                speed = defaultMoveSpeed;
            }
        }
        moveDirection.y -= (gravity * Time.deltaTime);
        controller.Move(Vector3.ClampMagnitude(moveDirection, speed) * Time.deltaTime); //Clamped to disallow quicker diagonal movement than straight - Simon Voss
    }
}