using UnityEngine;
using System.Collections;

public class TestLayer : MonoBehaviour {
    public GameObject[] m_arrGameObject;
	
    void ToggleActiveLayer(int ix )
    {
        m_arrGameObject[ix].SetActive(!m_arrGameObject[ix].activeSelf);
    }

    public void ToggleActive_0()
    {
        ToggleActiveLayer(0);
    }
    public void ToggleActive_1()
    {
        ToggleActiveLayer(1);
    }
    public void ToggleActive_2()
    {
        ToggleActiveLayer(2);
    }
    public void ToggleActive_3()
    {
        ToggleActiveLayer(3);
    }
    public void ToggleActive_4()
    {
        ToggleActiveLayer(4);
    }
    public void ToggleActive_5()
    {
        ToggleActiveLayer(5);
    }
    public void ToggleActive_6()
    {
        ToggleActiveLayer(6);
    }
}
