using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehaviour : MonoBehaviour
{
    public static bool isPaused = false; //belongs to class, not instance
    private KeyCode pauseKey = KeyCode.Escape;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if(!isPaused)
        {
            //if game is not paused, show menu
            
            pausePanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            pausePanel.SetActive(false);
            isPaused = false;
        }

    }

    /// <summary>
    /// Restarts the game at the beginning of the scene.
    /// But be careful, it does not reset static variables automatically.
    /// Will have to do that manually
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false; //need to reset bc static i guess idk
    }

    /// <summary>
    /// If this function is called, it is because the pause panel is already
    /// active, so no need to check if it's active or not. Sets the panel
    /// inactive and starts gameplay again
    /// </summary>
    public void Continue()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
