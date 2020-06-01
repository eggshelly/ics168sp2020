using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause()
    {
        if(pauseMenu!=null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void unpause()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void play()
    {
        SceneManager.LoadScene("main");
    }

    public void quit()
    {
        Application.Quit();
    }
}
