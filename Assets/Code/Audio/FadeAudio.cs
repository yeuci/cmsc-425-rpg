using System.Collections;
using UnityEngine;

public class FadeAudio : MonoBehaviour
{
    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Pause();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn (AudioSource audioSource, float FadeTime, float targetVolume) {
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < targetVolume) {
            audioSource.volume += targetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
