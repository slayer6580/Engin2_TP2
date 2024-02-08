using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject m_target;
    [SerializeField] private Vector3 m_buffer;

    private void Update()
    {
        transform.position = new Vector3(m_target.transform.position.x + m_buffer.x, m_target.transform.position.y + m_buffer.y, m_target.transform.position.z + m_buffer.z);
    }
}
