using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load("Game Manager") as GameObject).GetComponent<GameManager>();
        }
        return instance;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public PlayerCharacter playerCharacter;

    public void SetFPSInput(bool enabled)
    {
        if (playerCharacter != null)
        {
            playerCharacter.FPSInput.enabled = enabled;
        }
    }

    public void SetMouseLook(bool enabled)
    {
        if (playerCharacter != null)
        {
            playerCharacter.mouseLook.enabled = enabled;
        }
    }
}