using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Action[] actions = new Attack[4]; // 0 - attack, 1 - magic, 2 - run, 3 - potion
    public GameObject player, enemy;

    void Start()
    {
        if(player == null) player = GameObject.FindGameObjectWithTag("Player");
        if(enemy == null) enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void Animate(BattleOption option) {
        actions[(int)option].Execute(this, player, enemy);
    }
}
