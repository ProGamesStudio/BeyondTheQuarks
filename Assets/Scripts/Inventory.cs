using UnityEngine;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void SaveInventory(SaveData data)
    {
        data.inventoryItems = new List<string>(items);
    }

    public void LoadInventory(SaveData data)
    {
        items = new List<string>(data.inventoryItems);
    }
}
