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
        timer,
        noStamina,
        slideMiddle,
        spinning,
        Trap
        
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
            DesactivateMusic();
        }

    }

    /// <summary> Pour dire de jouer un son une fois pour tout les clients </summary>
    [Command(requiresAuthority = false)]
    public void CmdPlaySoundEffectsOneShotAll(ESound sound, Vector3 newPosition)
    {
        RpcPlaySoundEffectsOneShot(sound, newPosition);
    }

    /// <summary> Pour dire de jouer un son une fois pour seulement le joueur client </summary>
    [Command(requiresAuthority = false)]
    public void CmdPlaySoundEffectsOneShotTarget(ESound sound, Vector3 newPosition, NetworkIdentity player)
    {
        TargetPlaySoundEffectsOneShot(player.connectionToClient, sound, newPosition);
    }

    /// <summary> Pour dire de jouer un son en boucle (tout client) </summary>
    [Command(requiresAuthority = false)]
    public void CmdPlaySoundEffectsLoop(ESound sound, Vector3 newPosition)
    {
        RpcPlaySoundEffectsLoop(sound, newPosition);
    }

    /// <summary> Pour dire d'arreter de jouer un son en boucle </summary>
    [Command(requiresAuthority = false)]
    public void CmdStopSoundEffectsLoop(ESound sound, Vector3 newPosition)
    {
        RpcStopSoundEffectsLoop(sound, newPosition);
    }

    /// <summary> Tout les clients va jouer un son une fois </summary>
    [ClientRpc]
    public void RpcPlaySoundEffectsOneShot(ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        AudioClip clip = m_sounds[(int)sound];
        audiobox.m_isPlaying = true;
        MoveAudioBox(audiobox, newPosition);
        PlayClipOneShot(audiobox, sound);
        StartCoroutine(ReActivateAudioBox(audiobox, clip));
    }

    /// <summary> Le client va jouer un son une fois </summary>
    [TargetRpc]
    private void TargetPlaySoundEffectsOneShot(NetworkConnectionToClient target, ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        AudioClip clip = m_sounds[(int)sound];
        audiobox.m_isPlaying = true;
        MoveAudioBox(audiobox, newPosition);
        PlayClipOneShot(audiobox, sound);
        StartCoroutine(ReActivateAudioBox(audiobox, clip));
    }

    /// <summary> Tout les clients va jouer un son en boucle </summary>
    [ClientRpc]
    public void RpcPlaySoundEffectsLoop(ESound sound, Vector3 newPosition)
    {
        AudioBox audiobox = FindAValidAudioBox();

        if (audiobox == null)
            return;

        audiobox.m_isPlaying = true;
        MoveAudioBox(audiobox, newPosition);
        PlayClipLoop(audiobox, sound);
    }

    /// <summary> Tout les clients va arreter de jouer un son en boucle </summary>
    [ClientRpc]
    public void RpcStopSoundEffectsLoop(ESound sound, Vector3 newPosition)
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

    /// <summary> Pour trouver une audioBox en attente d'action </summary>
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

    /// <summary> Pour réactiver une audioBox selon le temps du clip </summary>
    private IEnumerator ReActivateAudioBox(AudioBox audiobox, AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        audiobox.m_isPlaying = false;
    }

    /// <summary> Pour bouger l'audioBox a un endroit précis </summary>
    private void MoveAudioBox(AudioBox audioBox, Vector3 newPosition)
    {
        audioBox.transform.position = newPosition;
    }

    /// <summary> Pour que l'audioBox joue le son une fois </summary>
    private void PlayClipOneShot(AudioBox audiobox, ESound sound)
    {
        AudioSource audioSource = audiobox.GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.PlayOneShot(m_sounds[(int)sound]);
    }

    /// <summary> Pour que l'audioBox joue le son en boucle </summary>
    private void PlayClipLoop(AudioBox audiobox, ESound sound)
    {
        AudioSource audioSource = audiobox.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = m_sounds[(int)sound];
        audioSource.Play();
    }

    /// <summary> Pour désactiver la musique du jeu </summary>
    private void DesactivateMusic()
    {
        GetComponent<AudioSource>().Stop();
    }



}
