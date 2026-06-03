using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    public GameObject newGamePanel;
    public Button noButton;
    public Button newGameButton;

    public void OnNewGamePressed()
    {
        if (SaveSystem.SaveExists())
        {
            ShowConfirm();
        }
        else
        {
            StartNewGame();
        }
    }

    public void ShowConfirm()
    {
        newGamePanel.SetActive(true);
        noButton.Select();
    }

    public void OnConfirmYes()
    {
        newGamePanel.SetActive(false);
        
        StartNewGame();
    }

    public void OnConfirmNo()
    {
        newGamePanel.SetActive(false);
        newGameButton.Select();
    }

    public void StartNewGame()
    {
        SaveSystem.DeleteSave();

        SceneManager.LoadScene("TutorialScene");
    }
}
