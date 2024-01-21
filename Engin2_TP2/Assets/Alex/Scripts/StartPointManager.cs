using UnityEngine;

public class StartPointManager : MonoBehaviour
{
    [SerializeField] private StartPoint[] m_startPoint;

    private static StartPointManager s_instance = null;

    public static StartPointManager GetInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;

        for (int i = 0; i < m_startPoint.Length; i++)        
            m_startPoint[i].SetActivation(false);
        
    }

    public Vector3 GetStartPointAndSetColor(GameObject _character)
    {
        for (int i = 0; i < m_startPoint.Length; i++)
        {
            if (m_startPoint[i].GetIsActivated() == false)
            {
                m_startPoint[i].SetActivation(true);
                _character.GetComponent<CharacterColor>().SetCharacterColor(i);      
                return m_startPoint[i].startPointTransform.position;
            }
        }
        return Vector3.zero;
    }

    [System.Serializable]
    public struct StartPoint
    {
        public Transform startPointTransform;
        private bool m_isActivated;

        public bool GetIsActivated()
        {
            return m_isActivated;
        }

        public void SetActivation(bool value)
        {
            m_isActivated = value;
        }
    }

}
