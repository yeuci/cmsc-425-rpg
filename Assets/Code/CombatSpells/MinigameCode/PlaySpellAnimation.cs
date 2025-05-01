using System.Collections;
using UnityEngine;

public abstract class PlaySpellAnimation : MonoBehaviour
{
    public GameObject spellPrefab;
    public Transform playerPosition;
    public Transform enemyPostion;
    public float damage;
    public DamagePopupGenerator damagePopupGenerator;
    protected AudioSource soundEffect;

    public abstract IEnumerator StartAnimation();

}
