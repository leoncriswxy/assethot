using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadFileTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        string[] urls = File.ReadAllLines($"{Application.dataPath}/urls.txt");

        //StartCoroutine(test(urls[0]));

        for (int i = 0; i < urls.Length; i++)
        {
            Debug.Log(urls[i]);
            DownloadUnit data = new DownloadUnit()
            {
                name = i.ToString(),
                downUrl = urls[i],
                savePath = $"{Application.streamingAssetsPath}/{i}",
                completeFun = (bb) => { Debug.Log(bb.name); },
                errorFun = (bb, s) => { Debug.Log(bb.name + "===" + s); },
                progressFun = (bb, s, b) => { Debug.Log(bb.name + "---" + s + "----" + b); }
            };
            DownloadMgr.I.DownloadAsync(data);
        }
    }
    IEnumerator test(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();
        if (uwr.isDone)
        {
            Debug.Log(uwr.downloadHandler.text);
            File.WriteAllBytes($"{Application.streamingAssetsPath}/ANDROID", uwr.downloadHandler.data);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
