using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BigNumber
{
    float[] m_ArrNumber = new float[27]; // 0, a~z
    public string m_strNumber;

    public void Add(string str)
    {
        // str.Substring();
        int ix = 0;
        int substringLengthCount = 0;
        string strtmp;
        for (int i=str.Length-1; i >= 0; i--)
        {
            // 숫자라면 Substring Length count 증가
            if ( (str[i] >= '0' && str[i] <= '9') || str[i] == '.')
            {
                substringLengthCount++;
            }
            // 단위 문자 이고 substringLengthCount 가 있다면 인식된 해당 단위에서 변환
            else if (str[i] >= 'a' && str[i] <= 'z')
            {
                strtmp = str.Substring(i + 1, substringLengthCount);
                // m_ArrNumber[ix] = ;
                ix = ((int)str[i] - (int)'a') + 1;
            }
        }
    }
	
}
