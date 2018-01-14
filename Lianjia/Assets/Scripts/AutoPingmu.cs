using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AutoPingmu : MonoBehaviour {


	private Vector4 baseClipTmp = Vector4.zero;
	private bool isFinish = false;
	void Start(){
		if(!isFinish){

			UIPanel panel = GetComponent<UIPanel>();
			if(baseClipTmp.Equals(Vector4.zero)){
				baseClipTmp = panel.baseClipRegion;
			}
            panel.baseClipRegion = new Vector4(baseClipTmp.x, baseClipTmp.y, Screen.width, Screen.height);

            isFinish = true;
		}
	}
}
