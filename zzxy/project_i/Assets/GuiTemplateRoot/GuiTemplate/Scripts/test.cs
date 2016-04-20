using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
        GuiMgr.Instance.Show<Gui_Login> (GuiMgr.ELayerType.Back, true, null);

        // 캐시레이어에 올릴 수도 있다.
        GuiMgr.Instance.Show<Gui_PopupMessage>(GuiMgr.ELayerType.Cache, true, null);
    }

    // Update is called once per frame
    void Update () {
	
		if (Input.GetKey (KeyCode.Alpha1)) {
            // back layer 로
			GuiMgr.Instance.Show<Gui_Login> (GuiMgr.ELayerType.Back, true, null);

        }
		if (Input.GetKey (KeyCode.Alpha2)) {
            // Front layer 로
            GuiMgr.Instance.Show<Gui_Login> (GuiMgr.ELayerType.Front, true, null);
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
            // Cache layer 로
            GuiMgr.Instance.Show<Gui_Login> (GuiMgr.ELayerType.Cache, true, null);
		}
        if (Input.GetKey(KeyCode.Alpha4))
        {
            GuiMgr.Instance.Show<Gui_PopupMessage>(GuiMgr.ELayerType.Front, true, null);
        }

    }
}
