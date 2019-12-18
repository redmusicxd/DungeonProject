using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    
    void BuildDatabase()
    {
        items = new List<Item>() {
                new Item(0, "Blaster Launcher", "A Nerf Like Gun",
                new Dictionary<string, int>
                {
                    {"Power", 30 },
                    {"Defence", 0 }
                })
        };
    }
}
