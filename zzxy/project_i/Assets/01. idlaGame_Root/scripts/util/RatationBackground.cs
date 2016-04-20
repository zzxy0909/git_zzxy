using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RatationBackground : MonoBehaviour {
    public float m_BoundayLeft = -90;
    public float m_RightOffset = 45;
    public Transform m_RootObject;
//    public RotationNode[] m_arrRotationNode;
    public float m_Speed = 1f;
    public Transform m_CurrentRoot;
    public Transform m_CurrentEnd;

    public bool m_isReady = true;

    // Use this for initialization
    void Start () {

        StartCoroutine(PlayRotation());
	}
    IEnumerator PlayRotation()
    {
        // 시스템 준비상테 확인. 각 메니져들 로딩 확인 등등.
        while(m_isReady == false)
        {
            yield return new WaitForEndOfFrame();
        }

        m_CurrentRoot.DOMoveX(m_BoundayLeft, m_Speed).SetEase(Ease.Linear).SetSpeedBased().OnComplete( () =>
        {
            ChangeRoot();
        });

        
    }
    void ChangeRoot()
    {
        // current 의 tail을 루트로 설정.
        Transform tmp = m_CurrentRoot.GetComponent<RotationNode>().m_tail;
        tmp.SetParent(m_RootObject);

        // m_CurrentRoot 는 end 로.
        // 미리 m_tail 설정. m_CurrentEnd.GetComponent<RotationNode>().m_tail = m_CurrentRoot;
        m_CurrentRoot.SetParent(m_CurrentEnd);
        m_CurrentRoot.localPosition = new Vector3(m_RightOffset, 0, 0);
        m_CurrentEnd = m_CurrentRoot;

        m_CurrentRoot = tmp;

        StartCoroutine(PlayRotation());
    }

    // Update is called once per frame
    void Update () {
	
	}
}
