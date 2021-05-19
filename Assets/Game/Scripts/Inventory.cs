using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    [SerializeField]
    public List<string> inventory;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void addToInventory(string item)
    {
        inventory.Add(item);
    }

    public void removeFromInventory(string item)
    {
        inventory.Remove(item);
    }

    public bool hasItem(string name)
    {
        return inventory.Contains(name);
    }
}