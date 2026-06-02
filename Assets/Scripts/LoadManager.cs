using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static SaveData loadedData;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadedData == null)
        {
            return;
        }

        // Load Inventory
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory != null)
        {
            inventory.LoadInventory(loadedData);
        }

        // Load Player
        Player player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            player.LoadPlayer(loadedData);
        }

        loadedData = null;
    }
}
