using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Eliott & Gabriel
delegate void Method();
public class PickUpObject : MonoBehaviour
{
    RaycastHit hit;
    float pickupRadius = 2;
    UnityEngine.UI.Text text;
    public Transform Player;
    [SerializeField] GameObject handObject;
    public Transform onHand;
    bool usingHand;

    void Start()
    {
        Player = GameObject.Find("FirstPersonCharacter").transform;
        onHand = GameObject.Find("Ipickthingsup").transform;
    }

    void Update()
    {
        //if (!GameManager.ins.inspecting)
        //{
        //    if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRadius))
        //    {
        //        if (hit.collider.gameObject == gameObject)
        //        {
        //            GameManager.ins.heldItem = gameObject;
        //            GetComponent<Rigidbody>().velocity = GameObject.FindGameObjectWithTag("MainCamera").transform.TransformDirection(Vector3.forward * 10);
        //            this.GetComponent<Rigidbody>().useGravity = false;
        //            this.transform.position = onHand.position;
        //            this.transform.localRotation = onHand.rotation;
        //            GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);

        //            this.transform.parent = Camera.main.transform;
        //            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //            handObject = gameObject;
        //            usingHand = true;
        //        }
        //    }

        //    if (Input.GetKeyUp(KeyCode.Mouse0) && usingHand)
        //    {
        //        transform.parent = null;
        //        GetComponent<Rigidbody>().useGravity = true;
        //        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //        handObject = null;
        //        usingHand = false;
        //        GameManager.ins.heldItem = null;
        //    }

        //    if (Input.GetKeyDown(KeyCode.Mouse1))
        //    {
        //        if (transform.parent != null && GameManager.ins.heldItem == gameObject)
        //        {
        //            transform.parent = null;
        //            GetComponent<Rigidbody>().useGravity = true;
        //            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //            GetComponent<Rigidbody>().velocity = GameObject.FindGameObjectWithTag("MainCamera").transform.TransformDirection(Vector3.forward * 10);
        //            GameManager.ins.heldItem = null;
        //        }
        //    }
        //}
        //else if (GameManager.ins.heldItem == gameObject)
        //{
        //    transform.Rotate(10 * Input.GetAxis("Mouse Y"), 0, 10 * Input.GetAxis("Mouse X"));
        //}
    }
    void InputExample(KeyCode key, Method method)
    {
        method();
    }
}

