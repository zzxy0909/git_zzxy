using UnityEngine;
using System.Collections;

public class GameUtil
{
    #region Singleton
    private static GameUtil _instance = null;
    public static GameUtil Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameUtil();
            }
            return _instance;
        }
    }
    #endregion Singleton

    public static int GetArrRandom(float[] arr)
    {
        float r = Random.Range(0f, 1f);
        float r_sum = 0;
        int ix = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            r_sum += arr[i];
            if (r < r_sum)
            {
                ix = i;
                break;
            }
        }
        return ix;
    }
}
