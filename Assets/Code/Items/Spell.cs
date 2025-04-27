using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Item/Spell")]
public class Spell : Item
{
    private void OnEnable()
    {
        type = ItemType.Spell;
        actionType = ActionType.Cast;
        stackable = false;
        maxStack = 1;
        
    }

}
