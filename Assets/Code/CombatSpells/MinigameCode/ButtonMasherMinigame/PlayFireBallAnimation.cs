using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayFireBallAnimation : PlaySpellAnimation
{
    public GameObject explosionPrefab;
    
    public override IEnumerator StartAnimation() {
        spellPrefab = gameObject;
        GameObject fireball = Instantiate(spellPrefab, playerPosition.position, playerPosition.rotation);

        BattleManager.instance.updatePlayerHealthAndManaBar();
        BattleManager.instance.updatePlayerHealthAndManaText();

        Rigidbody rb = fireball.GetComponent<Rigidbody>();

        if (rb != null)
        {   
            rb.AddForce(-fireball.transform.right * 10f, ForceMode.Impulse);
            while (enemyPostion.position.x - fireball.transform.position.x > 0) {
                yield return null;
            }

            Destroy(fireball);
            GameObject explosion = Instantiate(explosionPrefab, enemyPostion.position, Quaternion.identity);
            soundEffect = explosion.GetComponent<AudioSource>();

            if (soundEffect != null) {
                soundEffect.Play();
            }

            ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();

            BattleManager.instance.recalculateEnemyHealthBar();

            damagePopupGenerator.CreatePopUp(enemyPostion.position, damage.ToString(), enemyPostion.right, Color.red);

            while (particleSystem.isPlaying) {
                yield return null;
            }

            Destroy(explosion);
        }
        yield break;
    }
}
