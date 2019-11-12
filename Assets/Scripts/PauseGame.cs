using UnityEngine;

// Erik Neuhofer
public class PauseGame : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] Canvas canvas = null;

    void Start()
    {
        //pauseMenu.SetActive(false);
        canvas.gameObject.SetActive(false);
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
            //pauseMenu.SetActive(true);
            canvas.gameObject.SetActive(true);
            isActive = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isActive == true)
        {
            //pauseMenu.SetActive(false);
            Time.timeScale = 1;
            canvas.gameObject.SetActive(false);
            isActive = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
