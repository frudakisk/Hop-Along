using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controllerPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleControllerPanel(bool toggle)
    {
        controllerPanel.SetActive(toggle);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
