﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using System;

//click Lua/Build lua bundle
public class TesetAssetBundle : MonoBehaviour
{
    private const string scirpt =
        @"
    --主入口函数。从这里开始lua逻辑
    function Main()					
	    print('logic start')	 		
    end

    --场景切换通知
    function OnLevelWasLoaded(level)

        collectgarbage('collect')

        Time.timeSinceLevelLoad = 0
    end

    function OnApplicationQuit()

    end
";

    int bundleCount = int.MaxValue;
    string tips = null;

    IEnumerator CoLoadBundle(string name, string path)
    {
        using (WWW www = new WWW(path))
        {
            if (www == null)
            {
                Debugger.LogError(name + " bundle not exists");
                yield break;
            }

            yield return www;

            if (www.error != null)
            {
                Debugger.LogError(string.Format("Read {0} failed: {1}", path, www.error));
                yield break;
            }

            --bundleCount;
            LuaFileUtils.Instance.AddSearchBundle(name, www.assetBundle);
            www.Dispose();
        }
    }

    IEnumerator LoadFinished()
    {
        while (bundleCount > 0)
        {
            yield return null;
        }

        OnBundleLoad();
    }

    public IEnumerator LoadBundles()
    {
        string streamingPath = Application.streamingAssetsPath.Replace('\\', '/');

#if UNITY_5 || UNITY_2017 || UNITY_2018
#if UNITY_ANDROID && !UNITY_EDITOR
        string main = streamingPath + "/" + LuaConst.osDir + "/" + LuaConst.osDir;
#else
        string main = "file:///" + streamingPath + "/" + LuaConst.osDir + "/" + LuaConst.osDir;
#endif
        WWW www = new WWW(main);
        yield return www;

        AssetBundleManifest manifest = (AssetBundleManifest)www.assetBundle.LoadAsset("AssetBundleManifest");
        List<string> list = new List<string>(manifest.GetAllAssetBundles());
#else
        //此处应该配表获取
        List<string> list = new List<string>() { "lua.unity3d", "lua_cjson.unity3d", "lua_system.unity3d", "lua_unityengine.unity3d", "lua_protobuf.unity3d", "lua_misc.unity3d", "lua_socket.unity3d", "lua_system_reflection.unity3d" };
#endif
        bundleCount = list.Count;

        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i];

#if UNITY_ANDROID && !UNITY_EDITOR
            string path = streamingPath + "/" + LuaConst.osDir + "/" + str;
#else
            string path = "file:///" + streamingPath + "/" + LuaConst.osDir + "/" + str;
#endif
            string name = Path.GetFileNameWithoutExtension(str);
            StartCoroutine(CoLoadBundle(name, path));
        }

        yield return StartCoroutine(LoadFinished());
    }

    void Awake()
    {
        LuaFileUtils file = new LuaFileUtils();
        file.beZip = true;
#if UNITY_ANDROID && UNITY_EDITOR
        if (IntPtr.Size == 8)
        {
            throw new Exception("can't run this in unity5.x process for 64 bits, switch to pc platform, or run it in android mobile");
        }
#endif
        StartCoroutine(LoadBundles());
    }


    void OnBundleLoad()
    {
        LuaState state = new LuaState();
        state.Start();
        state.DoString("print('hello tolua#:'..tostring(Vector3.zero))");
        state.DoString(scirpt,"AB.cs");
        //state.DoFile("Main.lua");
        LuaFunction func = state.GetFunction("Main");
        func.Call();
        func.Dispose();
        state.Dispose();
        state = null;
    }
}
