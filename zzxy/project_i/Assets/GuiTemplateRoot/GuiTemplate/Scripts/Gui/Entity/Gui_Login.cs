using UnityEngine;
using System.Collections;


public class Gui_Login : GuiBase
{
    string _btnLogin = "Panel/btnLogin";

    public override void SetInspactor_InitTransList()
    {
        _usedTransListKey.Clear(); _usedTransListObject.Clear();
        // key - object 쌍으로 설정.
		Set_InitTransListKey(_btnLogin, LoginClick);

        //_usedTransListKey.Add(_btnLogin); _usedTransListObject.Add(this.transform.FindChild(_btnLogin));
        //Get_usedTrans(_btnLogin).GetComponent<UIButton>().onClick.Clear();
        //EventDelegate.Add(Get_usedTrans(_btnLogin).GetComponent<UIButton>().onClick, LoginClick);
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
    /// "로그인" 버튼 클릭시 호출...
    /// </summary>
    public void LoginClick()
    {
        Debug.Log("~~~~~~~~~~~ LoginClick !!!");


    }


}
