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
            Debug.LogError("Il y avait plus qu'une instance de AudioManager dans la sc�ne, FIX IT!");
            Destroy(this);
        }

        if (!isLocalPlayer)
        {
            GetComponent<AudioSource>().Stop();
        }
    

    }
    [Command(requiresAuthority = false)]
    public void PlaySoundEffects_CMD(ESound sound, Vector3 newPosition)
    {
        PlaySoundEffects_RPC(sound, newPosition);   
    }

    [ClientRpc]
    public void PlaySoundEffects_RPC(ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        AudioClip clip = m_sounds[(int)sound];
        audiobox.m_isPlaying = true;     
        MoveAudioBox_RPC(audiobox, newPosition);
        PlayClip_RPC(audiobox, sound);
        StartCoroutine(ReActivateAudioBox(audiobox, clip));
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


    private void PlayClip_RPC(AudioBox audiobox, ESound sound)
    {
        audiobox.GetComponent<AudioSource>().PlayOneShot(m_sounds[(int)sound]);    
    }

}
