using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [Header("Power du powerUp")]
    [SerializeField] private PlayerPowers.EPowers power;

    [Header("Après ramasage, le temps en seconde pour la réapparition")]
    [SerializeField] private float m_respawnTime;

    [Header("Pour la disparition, glisser le meshRenderer de l'objet")]
    [SerializeField] private MeshRenderer m_renderer;

    private SphereCollider m_sphereCollider;

    private void Awake()
    {
        m_sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerPowers character = other.GetComponent<PlayerPowers>();

        if (character == null)
            return;

        if (character.CanHavePower()) // Si le joueur peut avoir un pouvoir
        {
            character.GetPower(power); // lui donner ce pouvoir
            DisablePowerUp();
            StartCoroutine(DelayBeforeRespawn(m_respawnTime));
        }

    }

    /// <summary> Le délai avant réapparition </summary>
    IEnumerator DelayBeforeRespawn(float time)
    {
        yield return new WaitForSeconds(time);
        Respawn();
    }

    /// <summary> Désactiver l'objet </summary>
    private void DisablePowerUp()
    {
        m_renderer.enabled = false;
        m_sphereCollider.enabled = false;
    }

    /// <summary> Réapparition de l'objet </summary>
    private void Respawn()
    {
        m_renderer.enabled = true;
        m_sphereCollider.enabled = true;
    }

    /// <summary> Change le nom de l'objet </summary>
    private void SetName()
    {
        gameObject.name = power.ToString() + "PowerUp";
    }

    // pour voir les changements apres modifications de l'objet
    private void OnValidate()
    {
        m_renderer.material = Resources.Load<Material>("Power/" + power.ToString()); // change le matériel
        SetName();
    }
}
