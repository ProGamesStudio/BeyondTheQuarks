using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string playerName;
    public string sceneName;

    public float playerX;
    public float playerY;
    
    public int health;

    public List<string> inventoryItems = new List<string>();
}
