using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour
{
    [SerializeField] private Button buttonMenu;

    private void Awake() {
        
        buttonMenu.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
     SceneManager.LoadScene("MainMenu");
    }
}
