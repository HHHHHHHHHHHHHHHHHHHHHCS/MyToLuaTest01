using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class TestLuaThread : MonoBehaviour
{
    public Button resumeBtn, closeBtn;

    private LuaState luaState;
    private LuaThread thread;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(ResumeThread);
        closeBtn.onClick.AddListener(CloseThread);
    }

    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();
        luaState.LogGC = true;//显示GC回收的LOG日志


        var luaScript = Resources.Load<TextAsset>("TestLuaThread.lua");
        luaState.DoString(luaScript.text);

        LuaFunction func = luaState.GetFunction("Test");
        func.BeginPCall();
        func.PCall();
        thread = func.CheckLuaThread();
        thread.name = "MyLuaThread";
        func.EndPCall();
        func.Dispose();
        func = null;

        thread.Resume(10);
    }

    private void Update()
    {
        luaState.CheckTop();
        luaState.Collect();
    }

    private void ResumeThread()
    {
        int ret = -1;

        if (thread != null && thread.Resume(true, out ret) == (int) LuaThreadStatus.LUA_YIELD)
        {
            Debugger.Log("lua yield: " + ret);
        }
    }

    private void CloseThread()
    {
        if (thread != null)
        {
            thread.Dispose();
            thread = null;
        }
    }

    private void OnDestroy()
    {
        CloseThread();
        luaState.Dispose();
    }
}