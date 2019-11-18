using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    GameManager gameManager;

    public FPSInput FPSInput;
    public MouseLook mouseLook;

    private void Start()
    {
        gameManager = GameManager.Instance();
        gameManager.playerCharacter = this;
    }
}
