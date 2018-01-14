using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAutoLoadingBg : MonoBehaviour {

	/************************************************************************
	 * 自适应UISprite，UITexture.											*
	 * 需要设置Sprite的对其方式，并在Sprite的父对象上增加UIAnchor				*
	 * 默认为全屏适应，需要设置为半屏适应请将 isUseHalfScreen 的值设为 true.		*
	 * **********************************************************************/

	/// <summary>
	/// 半屏适应(true).
	/// </summary>

	public int loadingWight;
	public int loadingHeight;
	private float WightOnHeightForLoaidng{
		get{return (float)loadingWight / (float)loadingHeight; }
	}

	// 支持中文
	void Awake () {
		UISprite sprite = GetComponent<UISprite>();
		UITexture texture = GetComponent<UITexture>();

		
		if ((float)((float)Screen.width / (float)Screen.height) > WightOnHeightForLoaidng) {    //如果屏幕的宽高比大于
			if (sprite != null) {
//				sprite.width = (int)sprite.localSize.x + appendWidth;
				sprite.width = Screen.width;
				sprite.height = (int)((float)Screen.width / WightOnHeightForLoaidng);
			} else if (texture != null) {
//				texture.width = (int)texture.localSize.x + appendWidth;
				texture.width = Screen.width;
				texture.height=(int)((float)Screen.width / WightOnHeightForLoaidng);
			} else {
				Debug.LogError ("This script need a 'Sprite' or 'Texture' object!");
			}
		} else {
		}

	}
}
