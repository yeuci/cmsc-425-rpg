using UnityEngine;


[CreateAssetMenu(fileName = "BattleAction", menuName = "BattleAction/Action")]
public class Action : ScriptableObject
{
    public virtual void Execute(AnimationManager am, GameObject attacker, GameObject defender) {
        Debug.Log("Action Executed.");
    }
}
