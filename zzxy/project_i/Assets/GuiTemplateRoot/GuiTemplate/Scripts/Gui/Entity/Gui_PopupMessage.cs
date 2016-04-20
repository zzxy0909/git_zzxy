using UnityEngine;
using System.Collections;


public class Gui_PopupMessage : GuiBase
{
    string _btnType1OK = "Panel/Type1/btnOK";
    string _btnType2OK = "Panel/Type2/btnOK";
    string _btnType2Cancel = "Panel/Type2/btnCancel";
    string _PanelType1 = "Panel/Type1";
    string _PanelType2 = "Panel/Type2";
    string _LabelMsg = "Panel/Panel_Message/Label_Msg";
    public override void SetInspactor_InitTransList()
    {
        _usedTransListKey.Clear(); _usedTransListObject.Clear();
        // key - object 쌍으로 설정.
		Set_InitTransListKey(_btnType1OK, OKClick);
        Set_InitTransListKey(_btnType2OK, OKClick);
        Set_InitTransListKey(_btnType2Cancel, CancelClick);
        Set_InitTransListKey(_PanelType1);
        Set_InitTransListKey(_PanelType2);
        Set_InitTransListKey(_LabelMsg);

    }

    public override void OnCreate()
	{		
        //_usedTransList.Clear();
		
        //_usedTransList["Login Button"] = null;				
        //FindTrans();

//        EventDelegate.Add(_usedTransList["Login Button"].GetComponent<UIButton>().onClick, LoginClick );

//        _usedTransList["Login Button"].GetComponent<UIButton>().onClick.Add( () => { LoginClick(); };

	}

	
	public override void OnEnter()
	{
		//_usedTransList["Login Button"].gameObject.SetActive(false);
	}


	public override void OnLeave()
	{
	}


	public override void OnDelete()
	{
//		_usedTransList.Clear();
	}

    /// <summary>
    /// "OK" 버튼 클릭시 호출...
    /// </summary>
    public void OKClick()
    {
        Debug.Log("~~~~~~~~~~~ OKClick !!!");

        GuiMgr.Instance.Show<Gui_PopupMessage>(GuiMgr.ELayerType.Front, false, null);
    }
    /// <summary>
    /// "Cancel" 버튼 클릭시 호출...
    /// </summary>
    public void CancelClick()
    {
        Debug.Log("~~~~~~~~~~~ CancelClick !!!");
        GuiMgr.Instance.Show<Gui_PopupMessage>(GuiMgr.ELayerType.Front, false, null);

    }

}
