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
        {
            s_instance = this;
        }         
        else
        {
            Debug.LogError("Il y avait plus qu'une instance de StartPointManager dans la scène, FIX IT!");
            Destroy(this);
        }
                    
    }

    public int SetParentAndColor(GameObject _character)
    {
        float distance = Mathf.Infinity;
        int startPointIndex = 0;
    
        for (int i = 0; i < m_startPoint.Length; i++)
        {
            float tempDistance = Vector2.Distance(_character.transform.position, m_startPoint[i].position);

            if (tempDistance < distance)
            {
                startPointIndex = i;
                distance = tempDistance;
            }
        }

        _character.transform.SetParent(m_playerParent);
        return startPointIndex;
    }

 

}
