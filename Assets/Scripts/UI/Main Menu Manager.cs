using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject tutorialPanel_01;
    [SerializeField] private GameObject tutorialPanel_02;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetMainMenu();
    }

    public void SetMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(false);
    }

    public void SetLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        tutorialPanel_01.SetActive(false);
        tutorialPanel_02.SetActive(false);
    }

    public void SetTutorial01()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        tutorialPanel_01.SetActive(true);
        tutorialPanel_02.SetActive(false);
    }

    public void SetTutorial02()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
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

    public void QuitGame(){
        Application.Quit();
    }
}
