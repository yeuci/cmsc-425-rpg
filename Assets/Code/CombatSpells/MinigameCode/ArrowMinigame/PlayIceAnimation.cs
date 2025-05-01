using System.Collections;
using UnityEngine;

public class PlayIceAnimation : PlaySpellAnimation
{
    public override IEnumerator StartAnimation() {

        spellPrefab = gameObject;
        GameObject spell = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);

        ParticleSystem particleSystem = spell.GetComponentInChildren<ParticleSystem>();

        damagePopupGenerator.CreatePopUp(enemyPostion.position, damage.ToString(), enemyPostion.right, Color.red);
        if (particleSystem != null) {
            while (particleSystem.isPlaying) {
                yield return null;
            }
        }

        Destroy(spell);
        yield break;
    }
}
