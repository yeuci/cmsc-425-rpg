using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Action[] actions = new Attack[4]; // 0 - attack, 1 - magic, 2 - run, 3 - potion
    public GameObject player, enemy;

    void Start()
    {
        
    }

    public void Animate(BattleOption option) {
        actions[(int)option].Execute(this, player, enemy);
    }
}
