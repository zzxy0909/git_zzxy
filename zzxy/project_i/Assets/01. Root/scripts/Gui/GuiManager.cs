using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// GuiBase 를 상속받아 구성된  Gui Entity들을 관리함.
// 각각의 View Prefab을 Resources/Gui/ 에 두고 불러오며, AsserBundle 적용시 Resources manager에 의존 한다.
// Cache Layer 를 사용하려면 ELayerType.Cache 로 Show 하면 되고, 
// 최근 사용된 Gui Entity는 _guiEntityPools, _limitOfPools 로 관리된다.
public class GuiManager : MonoBehaviour
{

    #region Singleton

    private static GuiManager _instance = null;

    public static GuiManager Instance
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
    public List<GuiBase>					    _guiEntityPools = new List<GuiBase>();

    protected Dictionary<string, GuiBase>		_showGuiEntityList = new Dictionary<string,GuiBase>();

    public Transform _TransBack;
    public Transform _TransFront;
    public Transform _TransCache;
    UICamera _BackUICamera;

    string strCachePath = "Cache/Root";

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

        return Show_Name<T>(guiTypeName, pLayer, pShow, pParams);
    }
    public T Show_Name<T>(string guiTypeName, ELayerType pLayer, bool pShow, JSONNode pParams) where T : GuiBase
    { 
        if (pShow)
        {
            GuiBase guiBase;
            Transform parentTrans = null;
            switch (pLayer)
            {
                case ELayerType.Back:
                    {
                        parentTrans = _TransBack; // GameObject.Find("Gui/Back/Camera").transform;
                        //_BackUICamera.enabled = true;
                    }
                    break;

                case ELayerType.Front:
                    {
                        parentTrans = _TransFront; // GameObject.Find("Gui/Front/Camera").transform;
                        // 팝업 시 백 이밴트는 끈다.
                        //_BackUICamera.enabled = false;
                    }
                    break;
                case ELayerType.Cache:
                    {
                        parentTrans = _TransCache; // GameObject.Find("Cache").transform;
                    }
                    break;

            }

            // 이미 보이는 중이고 리턴. parentTrans 가 같다면
            guiBase = Display_SetParent(guiTypeName, parentTrans, pParams);
            if(guiBase != null)
            {
                return guiBase as T;
            }

            for ( int i = 0; i < _guiEntityPools.Count; ++i )
            {
                guiBase = _guiEntityPools[i];

                if (guiTypeName == guiBase.GetType().ToString())
                {
                    _showGuiEntityList.Add(guiTypeName, guiBase);
                    guiBase.SetParameter(pParams);
                    Display_SetParent(guiTypeName, parentTrans, pParams);

                    return guiBase as T;
                }
            }

            // 등록 된 것이 없다면 
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
                // cache로 옮긴다.
                guiBase.OnFinish();

                guiBase.transform.SetParent(_TransCache);
                guiBase.transform.localPosition = Vector3.zero;
                guiBase.transform.localRotation = Quaternion.identity;
                guiBase.transform.localScale = Vector3.one;

                UIAnchor anchortmp = guiBase.GetComponent<UIAnchor>();
                if (anchortmp)
                    anchortmp.uiCamera = _TransCache.GetComponent<Camera>();

                //guiBase.StopAllCoroutines();
                //guiBase.gameObject.SetActive(false);

                if(_guiEntityPools.Contains(guiBase) == false)
                {
                    _guiEntityPools.Add(guiBase);
                }
            }

            //if (pLayer == ELayerType.Front)
            //{
            //    // 팝업닫으면 백 이밴트는 다시 킨다.
            //    _BackUICamera.enabled = true;
            //}

            return guiBase as T;
        }
    }
    GuiBase Display_SetParent(string guiTypeName, Transform parentTrans, JSONNode pParams)
    {
        GuiBase guiBase = null;
        if (_showGuiEntityList.TryGetValue(guiTypeName, out guiBase))
        {
            if (parentTrans == guiBase.transform.parent)
            {
                return guiBase;
            }
            else
            {
                guiBase.transform.parent = parentTrans;
                guiBase.transform.localPosition = Vector3.zero;
                guiBase.transform.localRotation = Quaternion.identity;
                guiBase.transform.localScale = Vector3.one;
                guiBase.SetParameter(pParams);

                UIAnchor anchortmp = guiBase.GetComponent<UIAnchor>();
                if (anchortmp)
                    anchortmp.uiCamera = parentTrans.GetComponent<Camera>();
                return guiBase;
            }
        }
        return guiBase;
    }

    protected void Awake()
    {
        _instance = this;

        _TransBack = _instance.GetBackCamera().transform;
        _TransFront = _instance.GetFrontCamera().transform;
        _TransCache = _instance.transform.Find(strCachePath);
        _BackUICamera = _TransBack.GetComponent<UICamera>();

    }
    

    protected void OnDestroy()
    {
        StopAllCoroutines();

        _instance = null;
    }
}
