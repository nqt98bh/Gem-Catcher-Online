using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviourPunCallbacks
{
    // References to UI panels
    public GameObject pauseMenuUI;
    public GameObject resumeMenuUI;
    public GameObject settingsMenuUI;
    public GameObject exitMenuUI;

    // The buttons
    public Button buttonBackToLooby;
    public Button buttonResume;
    public Button buttonSettings;
    public Button buttonExit;

    // Keep track of the game state (paused or not)
    private bool isPaused = false;

    void Start()
    {
        // Initially, hide all menus
        pauseMenuUI.SetActive(false);
        resumeMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);

        // Set button listeners for each action
        buttonBackToLooby.onClick.AddListener(BackToLobby);
        buttonResume.onClick.AddListener(ResumeGame);
        buttonSettings.onClick.AddListener(OpenSettings);
        buttonExit.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        // Optional: Toggle Pause with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void BackToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            Time.timeScale = 1f;
            PhotonNetwork.LeaveRoom();
            Debug.Log("leave room");

        }
        else
        {
            Debug.LogWarning("Tried to leave room but was not in a room.");
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Back to Lobby");
        SceneManager.LoadScene("Game Lobby");

        //Destroy(gameObject);
    }
    // Function to show the pause menu
    public void PauseGame()
    {
        isPaused = true;

        pauseMenuUI.SetActive(true);  // Hide Pause button
        resumeMenuUI.SetActive(true);  // Show Resume button
        settingsMenuUI.SetActive(true);  // Show Settings button
        exitMenuUI.SetActive(true);     // Show Exit button

        Time.timeScale = 0f;  // Pause the game
    }

    // Function to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);  // Show the Pause button
        resumeMenuUI.SetActive(false);  // Hide Resume button
        settingsMenuUI.SetActive(false);  // Hide Settings button
        exitMenuUI.SetActive(false);  // Hide Exit button

        Time.timeScale = 1f;  // Resume the game
    }

    // Function to open the settings menu
    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        resumeMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
    }

    // Function to open the exit menu
    public void ExitGame()
    {
        pauseMenuUI.SetActive(false);
        resumeMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        exitMenuUI.SetActive(true);  // Show the exit menu

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Stop play mode in the editor
        #else
            Application.Quit();  // Quit the game in a build
        #endif
    }
}
