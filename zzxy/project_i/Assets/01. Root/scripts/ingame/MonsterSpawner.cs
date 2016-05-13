using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MonsterSpawner : MonoBehaviour
{
    public float _spawnCheckTime = 1f;      // The amount of time between each spawn.
    public float _spawnDelay = 3f;      // The amount of time before spawning starts.
    public int _count = 0;
    public bool _isPlay = false;
    public string _strPrefabName;
    public MonsterWaypoint[] m_arrNext;
    public float[] m_arrRatio;
    public float _DefaultSpeed = 5;

    // Use this for initialization
    void Start()
    {
        SpawnPlay(_strPrefabName, _count);  // MonsterSpawnerController 에서 관리 필요.
    }
    public void SpawnPlay(string str_name, int count)
    {
        _count = count;
        _strPrefabName = str_name;
        _isPlay = true;

    }
    float _NextCheckSpawnTime = 0f;
    void Update()
    {
        if (_isPlay == true && _count > 0
            && _NextCheckSpawnTime < Time.time
            )
        {
            _NextCheckSpawnTime = Time.time + _spawnCheckTime;
            Spawn();
        }

    }
    
    void Spawn()
    {
        _count--;
        GameObject obj = SpawnerPool.Spawn(_strPrefabName, transform.position, transform.rotation);
        Enemy etmp = obj.GetComponent<Enemy>();
        if(etmp != null)
        {
            etmp.Init();
            NextPlayMove(etmp);
        }

        //int ix = GetArrRandom(m_arrRatio);
        //obj.transform.DOMove(_arrTargetPos[ix].position, _DefaultSpeed).SetEase(Ease.Linear); //~~~~~~~~~~ monster info 의 speed 로 변경 필요.

    }
    public void NextPlayMove(Enemy enemy)
    {
        if (m_arrRatio == null || m_arrRatio.Length <= 0)
        {
            return;
        }
        int ix = GameUtil.GetArrRandom(m_arrRatio);
        MonsterWaypoint next_tmp = m_arrNext[ix];
        enemy.PlayPathWaypoint(next_tmp);

        //Transform target = next_tmp.transform;
        //float rspeed = Random.Range(0.5f, enemy.m_MonsterInfo.MOV_SPD);
        //enemy.transform.DOMove(target.position, rspeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(() =>
        //{
        //    en_tmp.PlayNextWaypoint(next_tmp);
        //});

    }
}
