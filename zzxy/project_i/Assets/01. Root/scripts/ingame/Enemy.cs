using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum EnemyStatus
{
    None,
    PathMove,

}

public class Enemy : MonoBehaviour {
    public MonsterInfo m_StartInfo;
    public MonsterInfo m_MonsterInfo;
    const int C_MAXPOINT = 10;
    public MonsterWaypoint[] m_PathWaypoint = new MonsterWaypoint[C_MAXPOINT];
    public EnemyStatus m_EnemyStatus = EnemyStatus.None;

    public void Init()
    {
        m_MonsterInfo.SetInfo(m_StartInfo);
        m_EnemyStatus = EnemyStatus.None;
        ClearPath();
    }
    void kill_init()
    {
        m_EnemyStatus = EnemyStatus.None;
        ClearPath();
    }
    void ClearPath()
    {
        for (int i = 0; i < C_MAXPOINT; i++)
        {
            m_PathWaypoint[i] = null;
        }
    }

    public void SetPathWaypoint(MonsterWaypoint start)
    {
        MonsterWaypoint tmp = start;
        m_PathWaypoint[0] = tmp;
        for (int i=1; i<C_MAXPOINT; i++)
        {
            if (tmp == null)
            {
                m_PathWaypoint[i] = null;
                break;
            }
            tmp = m_PathWaypoint[i] = tmp.GetNextWaypoint();
        }
    }

    public void PlayPathWaypoint(MonsterWaypoint start)
    {
        if (m_EnemyStatus == EnemyStatus.PathMove)
        {
            return;
        }

        SetPathWaypoint(start);
        StartCoroutine(_PlayPathWaypoint());
        
    }

    public bool _bMoveComplet = false;
    IEnumerator _PlayPathWaypoint()
    {
        m_EnemyStatus = EnemyStatus.PathMove;
        for (int i = 0; i < C_MAXPOINT; i++)
        {
            if (m_PathWaypoint[i] == null)
            {
                break;
            }
            Transform target = m_PathWaypoint[i].transform;
            float rspeed = Random.Range(0.5f, m_MonsterInfo.MOV_SPD);
            _bMoveComplet = false;
            transform.DOMove(target.position, rspeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(() =>
            {
                _bMoveComplet = true;
            });
            while (_bMoveComplet == false)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        // ~~~~~~~~~~~~~ test 용
        Kill();

        //if (m_NextWaypoint.m_arrNext == null || m_NextWaypoint.m_arrNext.Length <= 0)
        //{
        //    return;
        //}
        //int ix = GameUtil.GetArrRandom(m_NextWaypoint.m_arrRatio);
        //MonsterWaypoint next_tmp = m_NextWaypoint.m_arrNext[ix];
        //Transform target = next_tmp.transform;
        //float rspeed = Random.Range(0.5f, m_MonsterInfo.MOV_SPD);
        //transform.DOMove(target.position, rspeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(() =>
        //{
        //    m_NextWaypoint = m_NextWaypoint.GetNextWaypoint();
        //    PlayNextWaypoint(m_NextWaypoint);
        //});
    }

    public void Hurt(int val)
    {
        int hp_damage = (int)(m_MonsterInfo.DEF) - val;
        if (hp_damage < 0)
        {
            m_MonsterInfo.DEF = 0;
            m_MonsterInfo.HP += hp_damage;
        } else
        {
            m_MonsterInfo.DEF -= val;
        }

        if (m_MonsterInfo.HP <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        kill_init();
        SpawnerPool.Destroy(this.gameObject);
    }
}
