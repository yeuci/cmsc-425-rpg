using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "BattleAction/Run")]
public class Run : Action
{
    public override void Execute(AnimationManager am, GameObject attacker, GameObject defender)
    {
        base.Execute(am, attacker, defender);

        // RUN ANIMATION CODE -----

        Debug.Log("RUN");
    }
}
