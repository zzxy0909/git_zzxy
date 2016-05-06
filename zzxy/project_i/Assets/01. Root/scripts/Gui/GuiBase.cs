using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class GuiBase : MonoBehaviour
{

    protected bool								_isStarted = false;
    protected bool								_isFinished = false;

    protected JSONNode							_params = null;

//    public Dictionary<string, Transform> _usedTransList = new Dictionary<string, Transform>();
    
    public List<string> _usedTransListKey = new List<string>();
    public List<Transform> _usedTransListObject = new List<Transform>();

#if UNITY_EDITOR
    public bool _isInitTransList = true;
    void OnEnable_Editor()
    {
        if (_isInitTransList == false) return;
        _isInitTransList = false;
        SetInspactor_InitTransList();
    }
#endif
	public void Set_InitTransListKey(string strkey, EventDelegate.Callback callback)
	{
		// key - object 쌍으로 설정.
		_usedTransListKey.Add(strkey); _usedTransListObject.Add(this.transform.FindChild(strkey));
		Get_usedTrans(strkey).GetComponent<UIButton>().onClick.Clear();
		EventDelegate.Add(Get_usedTrans(strkey).GetComponent<UIButton>().onClick, callback);
	}
    public void Set_InitTransListKey(string strkey)
    {
        // key - object 쌍으로 설정.
        _usedTransListKey.Add(strkey); _usedTransListObject.Add(this.transform.FindChild(strkey));
    }

    public Transform Get_usedTrans(string str)
    {
        for (int i = 0; i < _usedTransListKey.Count; i++)
        {
            if (_usedTransListKey[i] == str)
            {
                return _usedTransListObject[i];
            }
        }
        return null;
    }

//=====================================================
	public void IsRender(bool pIsRender)
	{
		gameObject.SetActive(pIsRender);
	}

    protected void Awake()
    {
        OnCreate();
    }

    protected void Start()
    {
        _isStarted = true;
        _isFinished = false;

        OnEnter();
    }

    protected void OnEnable()
    {
#if UNITY_EDITOR
        OnEnable_Editor();
#endif

        if (_isStarted)
        {
            _isFinished = false;

            OnEnter();
        }
    }

    protected void OnDisable()
    {
        OnLeave();

        _params = null;
    }

    protected void OnDestroy()
    {
        OnDelete();
    }

    // FindTrans 는 SetInspactor_usedTransList 로 설정 하여 초기화시 부하를 없게 한다.
    //protected void FindTrans()
    //{
    //    RecursiveFind(transform, _usedTransList);
    //}

    //protected void RecursiveFind(Transform pTrans, Dictionary<string, Transform> pFindTransList)
    //{
    //    if (pFindTransList.ContainsKey(pTrans.name))
    //    {
    //        pFindTransList[pTrans.name] = pTrans;
    //    }

    //    int childCnt = pTrans.childCount;

    //    for (int i = 0; i < childCnt; ++i)
    //    {
    //        RecursiveFind(pTrans.GetChild(i), pFindTransList);
    //    }
    //}


    public bool IsFinished
    {
        get
        {
            return _isFinished;
        }
    }

    public void Finish()
    {
        _isFinished = true;
    }

    public void SetParameter(JSONNode pParams)
    {
        _params = pParams;
    }

    public virtual void SetInspactor_InitTransList()
    {

    }

    public virtual void OnCreate()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnFinish()
    {
        Finish();
    }

    public virtual void OnLeave()
    {
    }

    public virtual void OnDelete()
    {
    }

}
