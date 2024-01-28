using UnityEngine;
using System.Collections;

// Code Review:
// Veut-on une hiérarchie de classe? Avec powerup abstrait, speed endant de powerup?
// À voir
//
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

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowers character = other.GetComponent<PlayerPowers>(); 

        // Code review
        // Faire if (character==null) return à la place?
        //

        if (character) // Si c'est un objet qui a un script PlayerPowers
        {
            if (character.CanHavePower()) // Si le joueur peut avoir un pouvoir
            {
                character.GetPower(power); // lui donner ce pouvoir
                DisablePowerUp(); 
                StartCoroutine(DelayBeforeRespawn(m_respawnTime));  

                // Code Review
                // Penser à créer un objet powerup manager qui gère le respawn d'items. 
                // Le powerUp ne devrait pas gérer son propre respawn. SOLID.
                // On verra si on a le temps?
                //
            }
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
        // Code review:
        // Cette fonction est utilisée où exactement? Par quoi?
        // Pourquoi s'appelle-t-elle OnValidate?
        //
        m_renderer.material = Resources.Load<Material>("Power/" + power.ToString()); // change le matériel
        SetName();
    }
}
