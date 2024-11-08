using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonInstructions;
    [SerializeField] private Button buttonQuit;

    private void Awake() {
        
        buttonPlay.onClick.AddListener(PlayGame);
        buttonInstructions.onClick.AddListener(Instructions);
        buttonQuit.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
     SceneManager.LoadScene("MainLevel");
    }

    private void Instructions()
    {
     SceneManager.LoadScene("Instructions");
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application quit - won't close the editor");
    }
}
