using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void StartNewGame()
    {
        SaveSystem.DeleteSave();

        SceneManager.LoadScene("TutorialScene");
    }
}
