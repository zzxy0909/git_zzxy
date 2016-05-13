using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MonsterWaypoint : MonoBehaviour {
//    public List<Enemy> m_lstEnemy;
    public MonsterWaypoint[] m_arrNext;
    public float[] m_arrRatio;

    public MonsterWaypoint GetNextWaypoint()
    {
        if(m_arrNext == null || m_arrNext.Length <= 0)
        {
            return null;
        }
        int ix = GameUtil.GetArrRandom(m_arrRatio);
        return m_arrNext[ix];
        
    }
    
}
