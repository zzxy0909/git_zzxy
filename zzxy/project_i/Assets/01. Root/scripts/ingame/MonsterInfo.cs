using UnityEngine;
using System.Collections;
//using System;
//using System.Reflection;
//using UnityEngine.Internal;
//using UnityEngine.Scripting;

[System.Serializable]
public class MonsterInfo
{
    public int HP; // 체력
    public float DEF; // 방어력
    public float DEF_UP_RTO; // 방어력 자동 회복률
    public float MOV_SPD;
    public int ATK;

    public void SetInfo(MonsterInfo sor)
    {
        HP = sor.HP;
        DEF = sor.DEF;
        DEF_UP_RTO = sor.DEF_UP_RTO;
        MOV_SPD = sor.MOV_SPD;
        ATK = sor.ATK;

    }
}
