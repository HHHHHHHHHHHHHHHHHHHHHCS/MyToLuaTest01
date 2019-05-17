using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UnityEngine;

public class CallLuaFunction : MonoBehaviour
{
    private LuaState luaState;

    private LuaFunction Print;
    private LuaFunction Add;


    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();
        DelegateFactory.Init();
        string fullPath = Path.Combine(Application.dataPath, @"My/03_CallLuaFunction");
        luaState.AddSearchPath(fullPath);
        luaState.Require("CallLuaFunction");

        Call();
        Invoke();
        PushFunc();
        Delegate();
        LuaInvoke();
    }


    private void Call()
    {
        Print = luaState.GetFunction("tab.Print");
        Print.Call();
    }

    private void Invoke()
    {
        Add = luaState.GetFunction("tab.Add");
        var result = Add.Invoke<int, int, string>(1, 2);
        print(result);
    }

    private void PushFunc()
    {
        Add = luaState.GetFunction("tab.Add");
        Add.BeginPCall();
        Add.Push(123);
        Add.Push(456);
        Add.PCall();
        string str = (string) Add.CheckString();
        Add.EndPCall();
        print(str);
    }

    private void Delegate()
    {
        //DelegateFactory.Register();//一些自定义的要去这里添加
        Add = luaState.GetFunction("tab.PlusOne");
        Func<int, int> func = Add.ToDelegate<Func<int, int>>();
        int result = func(1);
        Debugger.Log(func(1));
    }

    private void LuaInvoke()
    {
        int num = luaState.Invoke<int, int>("tab.PlusOne", 9, true);
        Debugger.Log("luastate call return :{0}",num);
    }


    private void OnDestroy()
    {
        luaState?.Dispose();
        luaState = null;
    }
}