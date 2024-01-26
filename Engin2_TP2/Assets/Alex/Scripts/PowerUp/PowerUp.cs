using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PlayerPowers.EPowers power;
    [SerializeField] private float m_respawnTime;
    [SerializeField] private MeshRenderer m_renderer;
    private SphereCollider m_sphereCollider;

    private void Awake()
    {
        m_sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowers character = other.GetComponent<PlayerPowers>();

        if (character)
        {
            if (character.CanHavePower())
            {
                character.GetPower(power);
                DisablePowerUp();
                StartCoroutine(DelayBeforeRespawn(m_respawnTime));
            }
        }
    }

    IEnumerator DelayBeforeRespawn(float time)
    {
        yield return new WaitForSeconds(time);
        Respawn();
    }

    private void DisablePowerUp()
    {
       m_renderer.enabled = false;
       m_sphereCollider.enabled = false;
    }

    private void Respawn()
    {
        m_renderer.enabled = true;
        m_sphereCollider.enabled = true;
    }

    private void SetName()
    {
        gameObject.name = power.ToString() + "PowerUp";
    }

    private void OnValidate()
    {
        m_renderer.material = Resources.Load<Material>("Power/" + power.ToString());
        SetName();
    }
}
