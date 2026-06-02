using UnityEngine;
using UnityEngine.SceneManagement;

public class Resume : MonoBehaviour
{
    public void ResumeGame()
    {
        if (!SaveSystem.SaveExists())
        {
            return;
        }

        SaveData data = SaveSystem.LoadGame();

        if (data == null)
        {
            return;
        }

        LoadManager.loadedData = data;

        SceneManager.LoadScene(data.sceneName);
    }
}
