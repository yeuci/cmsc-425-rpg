using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ItemSave {
    public int count = 0;
    public string item = null;
    public Item itemData = null;

    public ItemSave (int c, string i, Item iD) {
        count = c;
        item = i;
        itemData = iD;
    }

    public ItemSave() {}
}