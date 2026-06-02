using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName = "";
    public int health = 10;

    public void SavePlayer(SaveData data)
    {
        data.playerName = playerName;
        data.health = health;

        data.playerX = transform.position.x;
        data.playerY = transform.position.y;
    }

    public void LoadPlayer(SaveData data)
    {
        playerName = data.playerName;
        health = data.health;

        transform.position = new Vector2(data.playerX, data.playerY);
    }
}
