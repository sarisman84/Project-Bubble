using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

// Erik Neuhofer
public class PauseGame : MonoBehaviour
{
    bool isActive = false;
   // [SerializeField] GameObject pauseMenu;
    [SerializeField] Canvas canvas;

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
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isActive == true)
        {
            //pauseMenu.SetActive(false);
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            isActive = false;
        }
    }
}
