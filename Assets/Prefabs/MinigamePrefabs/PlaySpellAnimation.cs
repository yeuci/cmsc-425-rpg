using System.Collections;
using UnityEngine;

public class PlaySpellAnimation : MonoBehaviour
{
    public GameObject spellPrefab;
    public Transform enemyPostion;

    public IEnumerator StartAnimation() {
        GameObject spell = Instantiate(spellPrefab, enemyPostion.position, Quaternion.identity);

        ParticleSystem particleSystem = spell.GetComponentInChildren<ParticleSystem>();

        Debug.Log(particleSystem);

        if (particleSystem != null) {
            while (particleSystem.isPlaying) {
                yield return null;
            }
        }

        Destroy(spell);
        yield break;
    }

}
