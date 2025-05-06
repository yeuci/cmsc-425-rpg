using System.Collections;
using UnityEngine;

public class PlayNukeAnimation : PlaySpellAnimation
{

    public override IEnumerator StartAnimation() {
        spellPrefab = gameObject;

        GameObject nuke = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);
        soundEffect = nuke.GetComponent<AudioSource>();
        if (soundEffect != null) {
            soundEffect.Play();
        }  

        ParticleSystem particleSystem = nuke.GetComponent<ParticleSystem>();

        BattleManager.instance.updatePlayerHealthAndManaBar();
        BattleManager.instance.updatePlayerHealthAndManaText();

        if (particleSystem != null) {

            BattleManager.instance.recalculateEnemyHealthBar();
            yield return new WaitForSeconds(1.5f);
            damagePopupGenerator.CreatePopUp(enemyPostion.position, damage.ToString(), enemyPostion.right, Color.red);

            while (particleSystem  != null && particleSystem.isPlaying) {
                yield return null;
            }
        }

        Destroy(nuke);
        yield break;
        
    }
    
}
