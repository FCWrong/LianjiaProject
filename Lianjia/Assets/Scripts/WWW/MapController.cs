using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class MapController : MonoBehaviour {

    private const string allUrl = "http://yuntuapi.amap.com/datasearch/local?";
    private const string districtUrl = "http://yuntuapi.amap.com/datasearch/statistics/district?";
    private const string getMapUrl = "http://restapi.amap.com/v3/staticmap?";
    private const string gaodeKey = "3be1ee3501b23d25af1bc28498764be2";
    private const string tableId = "5a5a3a46305a2a284b2e06ce";

    private List<MapPointData> MapPointList = new List<MapPointData>();
    private Dictionary<string, DistrictData> DistrictDataList = new Dictionary<string, DistrictData>();


    private void Start()
    {
        StartCoroutine(GetAllMaPoints("北京"));
        StartCoroutine(GetdistrictUrl("河北省", "北京"));
        StartCoroutine(GetMap(MapPointList.Count > 0 ? MapPointList[0].location : "116.481485,39.990464", "10", "800*600"));
    }

    /// <summary>
    /// 获取所有地图点
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetAllMaPoints(string city)
    {
        string url = allUrl + "key=" + gaodeKey + "&tableid=" + tableId + "&keywords=&" + "city=" + city;
        WWW www = new WWW(url);

        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.text);
            JsonData json = JsonMapper.ToObject(www.text);
            if (json != null)
            {
                int state = (int)json["status"];
                if (state == 1)
                {
                    JsonData datas = (JsonData)json["datas"];
                    if (datas != null)
                    {
                        foreach (JsonData data in datas)
                        {
                            MapPointData point = new MapPointData();
                            point.id = (string)data["_id"];
                            point.location = (string)data["_location"];
                            point.name = (string)data["_name"];
                            point.address = (string)data["_address"];
                            point.city = (string)data["_city"];
                            point.district = (string)data["_district"];
                            foreach (JsonData imageJson in data["_image"])
                            {
                                point.images.Add(imageJson.ToString());
                            }
                            MapPointList.Add(point);
                            Debug.Log("获取所有点：name=" + point.name);
                        }
                    }
                    
                }
            }
        }
    }
    /// <summary>
    /// 区检索
    /// </summary>
    /// <param name="province"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    private IEnumerator GetdistrictUrl(string province,string city)
    {
        string url = districtUrl + "key=" + gaodeKey + "&tableid=" + tableId + "&keywords=&province="+ province+"&" + "city=" + city;
        WWW www = new WWW(url);

        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.text);
            JsonData json = JsonMapper.ToObject(www.text);
            if (json != null)
            {
                int state = (int)json["status"];
                if (state == 1)
                {
                    JsonData datas = (JsonData)json["datas"];
                    if (datas != null)
                    {
                        foreach (JsonData data in datas)
                        {
                            DistrictData d = new DistrictData();
                            d.name= (string)data["name"];
                            d.count= (string)data["count"];
                            DistrictDataList.Add(d.name,d);
                            Debug.Log("区检索：name="+ d.name);
                        }
                    }

                }
            }
        }
    }
    /// <summary>
    /// 获取静态图片
    /// </summary>
    /// <param name="province"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    private IEnumerator GetMap(string location, string zoom,string size)
    {
        string url = getMapUrl + "key=" + gaodeKey + "&location=" + location + "&zoom=" + zoom + "&size=" + size;
        bool exists = Exits(url);
        WWW www = new WWW(url);

        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.texture != null)
            {
                SetTexture(www.texture);
                if (!exists)
                {
                    Save(www);
                }
            }
        }
    }
    public UITexture texture;
    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="img"></param>
    void SetTexture(Texture2D img)
    {
        if (texture != null)
        {
            texture.mainTexture = img;
            return;
        }
    }
    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="www"></param>
    void Save(WWW www)
    {
#if !UNITY_WEBPLAYER
        try
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            File.WriteAllBytes(Path(www.url), www.bytes);
        }
        catch (Exception e)
        {

        }
#endif
    }
    /// <summary>  
    /// 检测缓存是否存在  
    /// </summary>  
    bool Exits(string url)
    {
#if !UNITY_WEBPLAYER
        return File.Exists(Path(url));
#else
        return false;  
#endif
    }
    /// <summary>  
    /// 缓存的基础路径  
    /// </summary>  
    string BasePath
    {
        get
        {
            Debug.Log("Application--" + Application.dataPath+"/huancun");
            return Application.dataPath + "/huancun" + "/Images/";
        }
    }
    /// <summary>  
    /// 获取缓存路径  
    /// </summary>  
    string Path(string url)
    {
        return BasePath + "" + url.Trim() + ".img";
    }
    /// <summary>
    /// 通过区县获取地图点信息
    /// </summary>
    /// <param name="district"></param>
    /// <returns></returns>
    private List<MapPointData> GetMapPointByDistrict(string district)
    {
        List<MapPointData> list = new List<MapPointData>();
        foreach (MapPointData data in MapPointList)
        {
            if (data.district.Equals(district))
            {
                list.Add(data);
            }
        }
        return null;
    }

}
/// <summary>
/// 地图点
/// </summary>
public class MapPointData
{
    /// <summary>
    /// id
    /// </summary>
    public string id;
    /// <summary>
    /// 位置
    /// </summary>
    public string location;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 地址
    /// </summary>
    public string address;
    /// <summary>
    /// 城市
    /// </summary>
    public string city;
    /// <summary>
    /// 区县
    /// </summary>
    public string district;
    /// <summary>
    /// 图片
    /// </summary>
    public List<string> images;
}
/// <summary>
/// 去检索data
/// </summary>
public class DistrictData
{
    /// <summary>
    /// 区县名
    /// </summary>
    public string name;
    /// <summary>
    /// 个数
    /// </summary>
    public string count;
}



