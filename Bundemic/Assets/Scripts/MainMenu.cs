using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Tyler Scene");
    }

    public void Credits ()
    {
        SceneManager.LoadScene("credits");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("gameover");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("GC mainmenu scene");
    }
    public void Cutscene()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
