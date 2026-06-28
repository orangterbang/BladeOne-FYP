using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; set;}
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverMenuPanel;
    [SerializeField] private GameObject tutorialPanel_01;
    [SerializeField] private GameObject tutorialPanel_02;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isPaused = false;
        ClosePauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                //pause the game
                SetPauseMenu();
            }
            else 
            {
                //unpause the game
                ClosePauseMenu();
            }
        }
    }

    public void UpdateGameOver(GameObject losingActor)
    {
        SetGameOverMenu();
        Time.timeScale = 0.5f;
    }

    public void SetPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        gameOverMenuPanel.SetActive(false);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(false);

        isPaused = true;

        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        gameOverMenuPanel.SetActive(false);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(false);

        isPaused = false;

        Time.timeScale = 1.0f;
    }

    public void SetGameOverMenu()
    {
        pauseMenuPanel.SetActive(false);
        gameOverMenuPanel.SetActive(true);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(false);
    }

    public void SetTutorial01()
    {
        pauseMenuPanel.SetActive(false);
        gameOverMenuPanel.SetActive(false);
        tutorialPanel_01.SetActive(true);
        tutorialPanel_02.SetActive(false);
    }

    public void SetTutorial02()
    {
        pauseMenuPanel.SetActive(false);
        gameOverMenuPanel.SetActive(false);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(true);
    }

    public void ChangeSceneByName(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
