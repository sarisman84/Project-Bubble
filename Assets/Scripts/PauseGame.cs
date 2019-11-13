using UnityEngine;

// Erik Neuhofer
public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject[] pauseMenuObjectsToOpen = null;
    [SerializeField] GameObject helpPage = null;

    bool isActive = false;

    void Start()
    {
        //pauseMenu.SetActive(false);
        foreach (GameObject obj in pauseMenuObjectsToOpen)
        {
            obj.SetActive(false);
        }
        helpPage.SetActive(false);
    }

    void LateUpdate()
    {
        EnableGamePause();
    }

    public void EnableGamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isActive == false)
        {
            Time.timeScale = 0;
            foreach (GameObject obj in pauseMenuObjectsToOpen)
            {
                obj.SetActive(true);
            }
            isActive = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isActive == true)
        {
            Time.timeScale = 1;
            foreach (GameObject obj in pauseMenuObjectsToOpen)
            {
                obj.SetActive(false);
            }
            isActive = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OpenHelp()
    {
        helpPage.SetActive(!helpPage.activeSelf);
    }
}
