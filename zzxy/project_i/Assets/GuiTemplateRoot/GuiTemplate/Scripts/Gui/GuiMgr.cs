using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// GuiBase 를 상속받아 구성된  Gui Entity들을 관리함.
// 각각의 View Prefab을 Resources/Gui/ 에 두고 불러오며, AsserBundle 적용시 Resources manager에 의존 한다.
// Cache Layer 를 사용하려면 ELayerType.Cache 로 Show 하면 되고, 
// 최근 사용된 Gui Entity는 _guiEntityPools, _limitOfPools 로 관리된다.
public class GuiMgr : MonoBehaviour
{

    #region Singleton

    private static GuiMgr _instance = null;

    public static GuiMgr Instance
    {
        get
        {
            return _instance;
        }
    }

    #endregion Singleton


    public enum ELayerType
    {
        Back,
        Front,
        Cache
    }


	protected int								_limitOfPools = 5; // Hide후 _guiEntityPools에 남겨두는 최고 Gui 수
    protected List<GuiBase>					    _guiEntityPools = new List<GuiBase>();

    protected Dictionary<string, GuiBase>		_showGuiEntityList = new Dictionary<string,GuiBase>();
    protected Dictionary<string, GuiBase>       _hideGuiEntityList = new Dictionary<string, GuiBase>();

    public Transform _TransBack;
    public Transform _TransFront;
    public Transform _TransCache;
    UICamera _BackUICamera;

    string strCachePath = "Cache/Dummy";

    // NGUI input check
    public static bool CheckInputNGUI()
    {
        // RaycastHit hit = new RaycastHit();
		return UICamera.Raycast(Input.mousePosition); // NGUI 구젼: , out hit, );
    }

	public Camera GetBackCamera()
	{
		return transform.Find("Back/Camera").GetComponent<Camera>();
	}

	public UICamera GetBackUICamera()
	{
        return transform.Find("Back/Camera").GetComponent<UICamera>();
	}

	public Camera GetFrontCamera()
	{
        return transform.Find("Front/Camera").GetComponent<Camera>();
	}

	public UICamera GetFrontUICamera()
	{
        return transform.Find("Front/Camera").GetComponent<UICamera>();
	}
   
    public T Find<T>() where T : GuiBase
    {
        GuiBase guiBase;

        if (_showGuiEntityList.TryGetValue(typeof(T).ToString(), out guiBase))
        {
            return guiBase as T;
        }

        return null;
    }

    public T Show<T>(ELayerType pLayer, bool pShow, JSONNode pParams) where T : GuiBase
    {
        string guiTypeName = typeof(T).ToString();

        if (pShow)
        {
            GuiBase guiBase;
            Transform parentTrans = null;
            switch (pLayer)
            {
                case ELayerType.Back:
                    {
                        parentTrans = _TransBack; // GameObject.Find("Gui/Back/Camera").transform;
                        _BackUICamera.enabled = true;
                    }
                    break;

                case ELayerType.Front:
                    {
                        parentTrans = _TransFront; // GameObject.Find("Gui/Front/Camera").transform;
                        // 팝업 시 백 이밴트는 끈다.
                        _BackUICamera.enabled = false;
                    }
                    break;
                case ELayerType.Cache:
                    {
                        parentTrans = _TransCache; // GameObject.Find("Cache").transform;
                    }
                    break;

            }

            // 이미 보이는 중이고 리턴. parentTrans 가 같다면
            if (_showGuiEntityList.TryGetValue(guiTypeName, out guiBase))
            {
                if (parentTrans == guiBase.transform.parent)
                {
                    return guiBase as T;
                }
                else
                {
                    guiBase.transform.parent = parentTrans;
                    guiBase.transform.localPosition = Vector3.zero;
                    guiBase.transform.localRotation = Quaternion.identity;
                    guiBase.transform.localScale = Vector3.one;
                    guiBase.SetParameter(pParams);

                    UIAnchor anchortmp = guiBase.GetComponent<UIAnchor>();
					if(anchortmp)
						anchortmp.uiCamera = parentTrans.GetComponent<Camera>();
                    return guiBase as T;
                }
                
            }

            if (_hideGuiEntityList.TryGetValue(guiTypeName, out guiBase))
            {
                guiBase.StopAllCoroutines();
                guiBase.gameObject.SetActive(false);

                _hideGuiEntityList.Remove(guiTypeName);

                _showGuiEntityList.Add(guiTypeName, guiBase);

                guiBase.SetParameter(pParams);

                guiBase.gameObject.SetActive(true);

                return guiBase as T;
            }

            for ( int i = 0; i < _guiEntityPools.Count; ++i )
            {
                guiBase = _guiEntityPools[i];

                if (guiTypeName == guiBase.GetType().ToString())
                {
                    _guiEntityPools.RemoveAt(i);

                    _showGuiEntityList.Add(guiTypeName, guiBase);

                    guiBase.SetParameter(pParams);

                    guiBase.gameObject.SetActive(true);

                    return guiBase as T;
                }
            }

            GameObject guiGO = (GameObject)GameObject.Instantiate(Resources.Load(string.Format("Gui/{0}", guiTypeName)));

            guiGO.name = guiTypeName;
            guiGO.transform.parent = parentTrans;
            guiGO.transform.localPosition = Vector3.zero;
            guiGO.transform.localRotation = Quaternion.identity;
            guiGO.transform.localScale = Vector3.one;

            guiBase = (GuiBase)guiGO.GetComponent(guiTypeName);

            _showGuiEntityList.Add(guiTypeName, guiBase);

            guiBase.SetParameter(pParams);

            // UIAnchor 가 있다면 적용.
            UIAnchor anchortmp1 = guiBase.GetComponent<UIAnchor>();
			if(anchortmp1)
            	anchortmp1.uiCamera = parentTrans.GetComponent<Camera>();

            return guiBase as T;
        }
        else
        {
            GuiBase guiBase = null;
            if (_showGuiEntityList.TryGetValue(guiTypeName, out guiBase))
            {
                _showGuiEntityList.Remove(guiTypeName);

                _hideGuiEntityList.Add(guiTypeName, guiBase);

                guiBase.OnFinish();

                guiBase.StopAllCoroutines();
                guiBase.gameObject.SetActive(false);
            }

            if (pLayer == ELayerType.Front)
            {
                // 팝업닫으면 백 이밴트는 다시 킨다.
                _BackUICamera.enabled = true;
            }
            return guiBase as T;
        }
    }


    protected void Awake()
    {
        _instance = this;

        _TransBack = _instance.GetBackCamera().transform;
        _TransFront = _instance.GetFrontCamera().transform;
        _TransCache = _instance.transform.Find(strCachePath);
        _BackUICamera = _TransBack.GetComponent<UICamera>();
        InvokeRepeating("RefreshPoolQueue", 0.0f, 1.0f);

    }

	List<GuiBase> _goToPools = new List<GuiBase>();
    protected void Update()
    {
        if (_TransBack == null)
        {
            _TransBack = _instance.GetBackCamera().transform;
        }
        if (_TransFront)
        {
            _TransFront = _instance.GetFrontCamera().transform;
        }
        if (_TransCache)
        {
            _TransCache = _instance.transform.Find(strCachePath);
        }

        if (0 == _hideGuiEntityList.Count)
        {
            return;
        }
			
        foreach (GuiBase guiBase in _hideGuiEntityList.Values)
        {
            if (guiBase.IsFinished)
            {
				_goToPools.Add(guiBase);
            }
        }

		if (null != _goToPools)
        {
			foreach (GuiBase guiBase in _goToPools)
            {
                _hideGuiEntityList.Remove(guiBase.GetType().ToString());

				if (_guiEntityPools.Contains (guiBase) == false) {
					_guiEntityPools.Add (guiBase);
				}
            }
			_goToPools.Clear ();
        }
    }

    protected void OnDestroy()
    {
        StopAllCoroutines();

        _instance = null;
    }


    protected void RefreshPoolQueue()
    {
        if (0 == _guiEntityPools.Count)
        {
            return;
        }

        int delCount = _guiEntityPools.Count - _limitOfPools;

        if (delCount <= 0)
        {
            return;
        }

        List<GuiBase> removeList = null;

        foreach ( GuiBase guiBase in _guiEntityPools )
        {
            if (delCount > 0)
            {
                delCount--;

                if (null == removeList)
                {
                    removeList = new List<GuiBase>();
                }

                removeList.Add(guiBase);
            }
            else
            {
                break;
            }
        }

        if (null != removeList)
        {
            foreach (GuiBase guiBase in removeList)
            {
                _guiEntityPools.Remove(guiBase);

                GameObject.DestroyImmediate(guiBase.gameObject);
            }
        }
    }

}
