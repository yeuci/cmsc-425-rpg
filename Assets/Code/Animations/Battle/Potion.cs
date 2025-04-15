using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "BattleAction/Potion")]
public class Potion : Action
{
    public override void Execute(AnimationManager am, GameObject attacker, GameObject defender)
    {
        base.Execute(am, attacker, defender);

        // DRINK POTION ANIMATION CODE ----

        Debug.Log("Drink Potion");
    }
}
