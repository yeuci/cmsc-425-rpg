using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "BattleAction/Attack")]
public class Attack : Action
{
    public override void Execute(AnimationManager am, GameObject attacker, GameObject defender)
    {
        base.Execute(am, attacker, defender);

        // ATTACK ANIMATION CODE --

        Debug.Log("Attack");
    }
}
