using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class TestCoroutine : MonoBehaviour
{
    public Button startBtn, stopBtn, gcBtn;

    private LuaState luaState;

    private void Awake()
    {
        startBtn.onClick.AddListener(LuaStartCoroutine);
        stopBtn.onClick.AddListener(LuaStopCoroutine);
        gcBtn.onClick.AddListener(LuaGC);
    }

    private void Start()
    {
        luaState=new LuaState();
        luaState.Start();
        LuaBinder.Bind(luaState);//把unity一些高级功能跟lua虚拟机绑定起来

        //创建Looper 协程有用到
        LuaLooper looper = gameObject.AddComponent<LuaLooper>();
        looper.luaState = luaState;

        new LuaResLoader();
        TextAsset luaFile = Resources.Load<TextAsset>("testCoroutine.lua");
        luaState.DoString(luaFile.text, "TestLuaCoroutine.lua");

        DelegateFactory.Init();
        LuaFunction f = luaState.GetFunction("TestCoroutine");
        f.Call();

        f.Dispose();
        f = null;
    }

    public void LuaStartCoroutine()
    {
        LuaFunction func = luaState.GetFunction("StartDelay");
        func.Call();
        func.Dispose();
        func = null;
    }

    public void LuaStopCoroutine()
    {
        LuaFunction func = luaState.GetFunction("StopDelay");
        func.Call();
        func.Dispose();
        func = null;
    }

    public void LuaGC()
    {
        luaState.DoString("collectgarbage('collect')","TestCoroutine.cs");
        Resources.UnloadUnusedAssets();
        
    }
}
