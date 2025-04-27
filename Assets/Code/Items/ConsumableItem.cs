using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable Item", menuName = "Scriptable Item/Consumable Item")]
public class ConsumableItem : Item
{
    private void OnEnable()
    {
        actionType = ActionType.Consume;
        type =  ItemType.Consumable;
        consumable = true;
        stackable = true;
        maxStack = 3;
    }
}
