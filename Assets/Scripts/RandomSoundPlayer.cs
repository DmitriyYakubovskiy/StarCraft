using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float delayBetweenSounds = 1f;

    private AudioSource audioSource;
    private bool canPlay = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlayRandomSound();
    }

    public void PlayRandomSound()
    {
        if (!canPlay) return;

        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("Список аудиоклипов пуст или не задан");
            return;
        }

        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();

        StartCoroutine(WaitForNextPlay(audioSource.clip.length + delayBetweenSounds));
    }

    private IEnumerator WaitForNextPlay(float waitTime)
    {
        canPlay = false;
        yield return new WaitForSeconds(waitTime);
        canPlay = true;
    }
}
