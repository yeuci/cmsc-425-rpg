using System.Collections;
using UnityEngine;

public class PlayLightningAnimation : PlaySpellAnimation
{

    public override IEnumerator StartAnimation()
    {
        spellPrefab = gameObject;
        GameObject lightningBolt = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);

        ParticleSystem particleSystem = lightningBolt.GetComponent<ParticleSystem>();
        soundEffect = lightningBolt.GetComponent<AudioSource>();

        BattleManager.instance.updatePlayerHealthAndManaBar();
        BattleManager.instance.updatePlayerHealthAndManaText();

        if (particleSystem != null) {
            if (soundEffect != null) {
                soundEffect.Play();
            }

            BattleManager.instance.recalculateEnemyHealthBar();
            damagePopupGenerator.CreatePopUp(enemyPostion.position, damage.ToString(), enemyPostion.right, Color.red);

            while (particleSystem  != null && particleSystem.isPlaying) {
                yield return null;
            }
        }

        Destroy(lightningBolt);
        yield break;
    }
}
