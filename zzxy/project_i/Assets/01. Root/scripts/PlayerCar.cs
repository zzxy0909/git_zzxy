using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerCar : MonoBehaviour {
    public Transform m_FartRoot;
    public Transform m_StartFartPos;
    public GameObject m_FartEffect;

    // Use this for initialization
    void Start () {
        m_FartRoot.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            if(GuiManager.CheckInputNGUI() == false)
            {
                PlayFart();
            }
        }
	
	}
    bool _isPlayFart = false;
    public float m_PlayFartTime = 3f;
    Vector3 _movePos1 = new Vector3(-2, 1, 0);
    Vector3 _movePos2 = new Vector3(-4, 3, 0);
    Vector3 _movePos3 = new Vector3(-6, 6, 0);

    public void PlayFart()
    {
 //       Debug.Log("~~~~~~~~~~~~~ PlayFart");
        if(_isPlayFart)
        {
            return;
        }
        _isPlayFart = true;

        StartCoroutine(PlayFartEffect());

        m_FartRoot.position = m_StartFartPos.position;
        m_FartRoot.gameObject.SetActive(true);
        m_FartRoot.DOMove(m_StartFartPos.position + _movePos1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_FartRoot.DOMove(m_StartFartPos.position + _movePos2, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_FartRoot.DOMove(m_StartFartPos.position + _movePos3, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    m_FartRoot.gameObject.SetActive(false);
                    _isPlayFart = false;
                });
            });
        });
    }

    public float m_FartEffectDur = 0.1f;
    IEnumerator PlayFartEffect()
    {
        m_FartEffect.SetActive(true);
        yield return new WaitForSeconds(m_FartEffectDur);
        m_FartEffect.SetActive(false);

    }
}
