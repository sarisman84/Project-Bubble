using UnityEngine;

// Erik Neuhofer - Simon Voss
public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject[] pauseMenuObjectsToOpen = null;

    public bool canPause;
    private bool isActive = false;

    CursorLockMode originalLockMode;

    void Start()
    {
        foreach (GameObject obj in pauseMenuObjectsToOpen)
        {
            obj.SetActive(false);
        }
        //helpPage.SetActive(false);
    }

    //void LateUpdate()
    //{
    //    EnableGamePause();
    //}

    //public void EnableGamePause()
    //{
    //    if (canPause)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape) && isActive == false) // Pauses the Game
    //        {
    //            originalLockMode = Cursor.lockState;
    //            Time.timeScale = 0;
    //            foreach (GameObject obj in pauseMenuObjectsToOpen)
    //            {
    //                obj.SetActive(true);
    //            }
    //            isActive = true;
    //            Cursor.lockState = CursorLockMode.None;
    //        }

    //        else if (Input.GetKeyDown(KeyCode.Escape) && isActive == true) // Unpauses the Game
    //        {
    //            Time.timeScale = 1;
    //            foreach (GameObject obj in pauseMenuObjectsToOpen)
    //            {
    //                obj.SetActive(false);
    //            }
    //            isActive = false;
    //            Cursor.lockState = originalLockMode;
    //        }
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                DoUnpause();
            }
            else
            {
                DoPause();
            }
        }
    }

    bool isPaused = false;
    public void DoPause()
    {
        //if (canPause)
        //{
            originalLockMode = Cursor.lockState;
            Time.timeScale = 0;
            foreach (GameObject obj in pauseMenuObjectsToOpen)
            {
                obj.SetActive(true);
            }
            isActive = true;
            Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        //}
    }

    public void DoUnpause()
    {
        Time.timeScale = 1;
        foreach (GameObject obj in pauseMenuObjectsToOpen)
        {
            obj.SetActive(false);
        }
        isActive = false;
        Cursor.lockState = originalLockMode;
        isPaused = false;
    }



    //public void OpenHelp() // Opens Help Page For Gameplay Controlls
    //{
    //    helpPage.SetActive(!helpPage.activeSelf);
    //}
}
