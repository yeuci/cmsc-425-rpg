using System.Collections;
using UnityEngine;

public class PlayIceAnimation : PlaySpellAnimation
{
    public override IEnumerator StartAnimation() {

        spellPrefab = gameObject;
        
        GameObject spell = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);
        soundEffect = spell.GetComponent<AudioSource>();

        if (soundEffect != null) {
            soundEffect.Play();
        }

        ParticleSystem particleSystem = spell.GetComponentInChildren<ParticleSystem>();

        BattleManager.instance.recalculateEnemyHealthBar();
        BattleManager.instance.updatePlayerHealthAndManaBar();
        BattleManager.instance.updatePlayerHealthAndManaText();
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
