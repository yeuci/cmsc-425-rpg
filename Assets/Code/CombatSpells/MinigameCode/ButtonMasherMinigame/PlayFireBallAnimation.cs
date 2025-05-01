using System.Collections;
using UnityEngine;

public class PlayFireBallAnimation : PlaySpellAnimation
{
    GameObject explosionPrefab;
    
    public override IEnumerator StartAnimation() {
        spellPrefab = gameObject;
        GameObject fireball = Instantiate(spellPrefab, playerPosition.position, playerPosition.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();

        if (rb != null)
        {   
            rb.AddForce(-fireball.transform.right * 10f, ForceMode.Impulse);
            while (enemyPostion.position.x - fireball.transform.position.x > 0) {
                yield return null;
            }

            Destroy(fireball);
            damagePopupGenerator.CreatePopUp(enemyPostion.position, damage.ToString(), enemyPostion.right, Color.red);
        }
        yield break;
    }
}
