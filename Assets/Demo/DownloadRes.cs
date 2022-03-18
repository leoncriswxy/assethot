using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using QAssetBundle;

public class DownloadRes : MonoBehaviour
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public string FTPServerUrl = "http://42.194.143.128:8888/down/3GmcWjeoGCBr";
    public string Patch = "Patch.xml";
    public string LocalPatch { get => Application.persistentDataPath + "/Res/Patch.xml"; }

    public string BundlePath { get => Application.persistentDataPath + "/Res/"; }

    public string urlMD5 = "http://42.194.143.128:8888/down/GfsdvtyukE1k";
    public string urlVersion = "http://42.194.143.128:8888/down/JD1eLer2ce8x";

    public TextAsset currentVersion;

    // Start is called before the first frame update
    void Start()
    {
        ResKit.Init();
        Debug.Log(currentVersion.text);
        StartCoroutine(CheackUpdateVersion());

        //StartCoroutine(DownloadResAB());
    }

    IEnumerator CheackUpdatePatch(string version)
    {
        //string url = $"{FTPServerUrl}/{RuntimePlatform.Android}/{version}/{Patch}";
        string url = $"http://42.194.143.128:8888/down/CNEeI7I0Ct24";
        Debug.Log(url);
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();
        if (uwr.isDone)
        {
            File.WriteAllBytes(LocalPatch, uwr.downloadHandler.data);
            var data = BinarySerializeOpt.XmlDeserialize<Pathces>(LocalPatch);
            Debug.Log(data.Version);
        }
    }

    IEnumerator CheackUpdateVersion()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(urlVersion);
        yield return uwr.SendWebRequest();
        if (uwr.isDone)
        {
            Debug.Log($"服务器版本信息：{uwr.downloadHandler.text}");
            string version = uwr.downloadHandler.text.Split(';')[0].Split('|')[1];
            StartCoroutine(CheackUpdatePatch(version));
        }

    }

    IEnumerator DownloadResAB()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://42.194.143.128:8888/down/X4qXRhx6I81T");
        yield return uwr.SendWebRequest();
        if (uwr.isDone)
        {
            Debug.Log(BundlePath + Map_prefab.BundleName);
            File.WriteAllBytes(BundlePath + Map_prefab.BundleName, uwr.downloadHandler.data);
            //构造文件流
            //FileStream fs = File.Create(_BundlePath + Map_prefab.BundleName);
            //将字节流写入文件里,request.downloadHandler.data可以获取到下载资源的字节流

            //fs.Write(uwr.downloadHandler.data, 0, uwr.downloadHandler.data.Length);
            //fs.Flush();     //文件写入存储到硬盘
            //fs.Close();     //关闭文件流对象
            //fs.Dispose();   //销毁文件对象
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        //传入这个AB包存在本地的路径加载本地资源
        AssetBundle ab = AssetBundle.LoadFromFile(BundlePath + Map_prefab.BundleName);
        for (int i = 0; i < ab.GetAllAssetNames().Length; i++)
        {
            Debug.Log(ab.GetAllAssetNames()[i]);
        }
        //获取可使用AssetBundle这个类里面的LoadAsset<T>(string name)方法获取资源
        GameObject wallPrefab = ab.LoadAsset<GameObject>(Map_prefab.MAP);

        Instantiate(wallPrefab);//实例化这个游戏物体
    }

    // Update is called once per frame
    void Update()
    {

    }
}
