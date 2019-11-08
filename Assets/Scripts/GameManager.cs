using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Gabriel & Eliott
public class GameManager : MonoBehaviour
{
    public GameObject heldItem;
    public static GameManager ins;
    public bool inspecting = false;

    void Start()
    {
        if (ins == null) 
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager gm = GameManager.ins;
            if (GameManager.ins.heldItem != null)
            {
                gm.inspecting = !gm.inspecting;
            }
        }
    }
}