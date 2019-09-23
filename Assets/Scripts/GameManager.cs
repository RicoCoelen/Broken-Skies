using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject deathPanel;
    private bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        deathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        deathPanel.SetActive(true);
        ToggleTime();
    }

    public void ToggleTime()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    public void Retry()
    {
        ToggleTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
