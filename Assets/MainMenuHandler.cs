using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public InputField playerInput;
    public Text playernameTxt;
    public GameObject renameButton;
    public Button startButton;
    public string playerName;

    bool isName;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (playerInput.text != null && Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                playerName = playerInput.text;
                playernameTxt.text = $"Hello {playerName}";

                playerInput.enabled = false;
                renameButton.SetActive(true);
                startButton.interactable = true;
                isName = true;
            }
            else if (!isName)
            {
                playernameTxt.text = null;
                startButton.interactable = false;
            }
        }
    }

    public void ChangeUserName()
    {
        renameButton.SetActive(false);
        isName = false;
        playerInput.enabled = true;
        playerInput.text = null;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(this);
    }

}
