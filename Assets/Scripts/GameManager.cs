using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of: " + this + " , was tried to be instantiated, but was destroyed! This instance was tried to be instantiated on: " + this.gameObject);
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] FPSInput fpsControl;
    [SerializeField] MouseLook mouseControl;

    private void Start()
    {
        if (!fpsControl || !mouseControl)
        {
            Debug.LogWarning("Missing connections in Game manager");
        }
    }

    public void SetFPSControlState(bool state)
    {
        fpsControl.enabled = state;
    }

    public void SetMouseControlState(bool state)
    {
        mouseControl.enabled = state;
    }
}