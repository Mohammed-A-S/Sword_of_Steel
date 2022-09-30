using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    public static bool game_is_pause = false;

    public GameObject PauseUI;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(game_is_pause == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                PauseUI.SetActive(false);
                Time.timeScale = 1f;
                game_is_pause = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PauseUI.SetActive(true);
                Time.timeScale = 0f;
                game_is_pause = true;
            }
        }
    }

    public void resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        game_is_pause = false;
    }
    public void replay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void menu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
