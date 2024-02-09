using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mirror;

public class AudioManager : NetworkBehaviour
{
    public enum ESound
    {
        jump,
        bumper,
        canon,
        checkpoint,
        deathZone,
        timer
    }

    [Header("Mettre tout les audios ici, regarder le Tooltip pour savoir l'ordre")]
    [Tooltip("jump\nbumper\ncanon\ncheckpoint\ndeathZone\ntimer")]
    [SerializeField] private List<AudioClip> m_sounds;

    [Header("Mettre tout les audioBox ici")]
    [SerializeField] private List<AudioBox> m_audioBox;

    private static AudioManager s_instance = null;

    public static AudioManager GetInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Debug.LogError("Il y avait plus qu'une instance de AudioManager dans la scène, FIX IT!");
            Destroy(this);
        }

        if (!isLocalPlayer)
        {
            GetComponent<AudioSource>().Stop();
        }


    }
    [Command(requiresAuthority = false)]
    public void PlaySoundEffectsOneShot_CMD(ESound sound, Vector3 newPosition)
    {
        PlaySoundEffectsOneShot_RPC(sound, newPosition);
    }

    [Command(requiresAuthority = false)]
    public void PlaySoundEffectsLoop_CMD(ESound sound, Vector3 newPosition)
    {
        PlaySoundEffectsLoop_RPC(sound, newPosition);
    }

    [Command(requiresAuthority = false)]
    public void StopSoundEffectsLoop_CMD(ESound sound, Vector3 newPosition)
    {
        PlaySoundEffectsLoop_RPC(sound, newPosition);
    }

    [ClientRpc]
    public void PlaySoundEffectsOneShot_RPC(ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        AudioClip clip = m_sounds[(int)sound];
        audiobox.m_isPlaying = true;
        MoveAudioBox_RPC(audiobox, newPosition);
        PlayClipOneShot_RPC(audiobox, sound);
        StartCoroutine(ReActivateAudioBox(audiobox, clip));
    }

    [ClientRpc]
    public void PlaySoundEffectsLoop_RPC(ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        audiobox.m_isPlaying = true;
        MoveAudioBox_RPC(audiobox, newPosition);
        PlayClipLoop_RPC(audiobox, sound);
    }

    [ClientRpc]
    public void StopSoundEffectsLoop_RPC(ESound sound, Vector3 newPosition)
    {
        AudioClip clip = m_sounds[(int)sound];
        foreach (AudioBox audioBox in m_audioBox)
        {
            AudioSource audioSource = audioBox.GetComponent<AudioSource>();
            if (audioBox.transform.position == newPosition && audioSource.clip == clip)
            {
                audioBox.m_isPlaying = false;
                audioSource.Stop();
                return;
            }
        }
        Debug.LogError("Pas d'audio a désactivé, AUDIO MANAGER script");
    }

    private AudioBox FindAValidAudioBox()
    {
        foreach (AudioBox audioBox in m_audioBox)
        {
            if (!audioBox.m_isPlaying)
            {
                return audioBox;
            }
        }
        Debug.LogError("Pas assez d'audioBox, en rajouter");
        return null;
    }

    private IEnumerator ReActivateAudioBox(AudioBox audiobox, AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        audiobox.m_isPlaying = false;
    }


    private void MoveAudioBox_RPC(AudioBox audioBox, Vector3 newPosition)
    {
        audioBox.transform.position = newPosition;
    }


    private void PlayClipOneShot_RPC(AudioBox audiobox, ESound sound)
    {
        audiobox.GetComponent<AudioSource>().PlayOneShot(m_sounds[(int)sound]);
    }

    private void PlayClipLoop_RPC(AudioBox audiobox, ESound sound)
    {
        AudioSource audioSource = audiobox.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = m_sounds[(int)sound];
        audioSource.Play();
    }

}
