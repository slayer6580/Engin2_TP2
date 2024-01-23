using UnityEngine;

public class StartPointManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_startPoint;
    [SerializeField] private Transform m_playerParent;

    private static StartPointManager s_instance = null;

    public static StartPointManager GetInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this; 
    }

    public int SetParentAndColor(GameObject _character)
    {
        float distance = Mathf.Infinity;
        int index = 0;
    
        for (int i = 0; i < m_startPoint.Length; i++)
        {
            float tempDistance = Vector2.Distance(_character.transform.position, m_startPoint[i].position);

            if (tempDistance < distance)
            {
                index = i;
                distance = tempDistance;
            }
        }

        _character.transform.SetParent(m_playerParent);
        return index;
    }

 

}
