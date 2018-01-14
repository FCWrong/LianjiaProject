using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour {


    public GameObject beijing;
    public GameObject dongcheng;
    public GameObject gongshang;

    public void OpenDongcheng()
    {
        beijing.SetActive(false);
        dongcheng.SetActive(true);
        
    }

    public void OpenBeijing()
    {
        beijing.SetActive(true);
        dongcheng.SetActive(false);
        gongshang.SetActive(false);

    }

    public void OnGongshang(GameObject obj)
    {
        GameObject t = obj.transform.Find("lianjia").gameObject;
        if (t.activeSelf)
        {
            t.SetActive(false);
        }
        else
        {
            t.SetActive(true);
        }
    }

    public void OnOpenGongshang()
    {
        gongshang.SetActive(true);
    }
}
