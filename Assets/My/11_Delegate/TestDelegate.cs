using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class TestDelegate : MonoBehaviour
{
    private LuaState luaState;
    private TestEventListener listener;

    private LuaFunction setClick1, addClick1,addClick2,removeClick1,removeClick2
        ,testOverride,removeEvent,addEvent,addSelfClick,removeSelfClick,callOnClick;

    private void Awake()
    {
        luaState = new LuaState();
        luaState.Start();
        
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();

        luaState.LogGC = true;
        var scirpt = Resources.Load<TextAsset>("TestDelegate.lua");
        luaState.DoString(scirpt.text);

        GameObject go = new GameObject("GoTest");
        listener = go.AddComponent<TestEventListener>();

        setClick1 = luaState.GetFunction("SetClick1");
        addClick1 = luaState.GetFunction("AddClick1");
        addClick2 = luaState.GetFunction("RemoveClick1");
        removeClick2 = luaState.GetFunction("RemoveClick2");
        testOverride = luaState.GetFunction("TestOverride");
        addEvent = luaState.GetFunction("AddEvent");
        removeEvent = luaState.GetFunction("RemoveEvent");
        addSelfClick = luaState.GetFunction("AddSelfClick");
        removeSelfClick = luaState.GetFunction("RemoveSelfClick");
        callOnClick = luaState.GetFunction("CallOnClick");
    }

    private void CallLuaFunction(LuaFunction func)
    {
        func.BeginPCall();
        func.Push(listener);
        func.PCall();
        func.EndPCall();
    }


    void Update()
    {
        luaState.Collect();
        luaState.CheckTop();
    }

    void SafeRelease(ref LuaFunction luaRef)
    {
        if (luaRef != null)
        {
            luaRef.Dispose();
            luaRef = null;
        }
    }

    private void OnDestroy()
    {
        SafeRelease(ref addClick1);
        SafeRelease(ref addClick2);
        SafeRelease(ref removeClick1);
        SafeRelease(ref removeClick2);
        SafeRelease(ref setClick1);
        SafeRelease(ref testOverride);
        SafeRelease(ref callOnClick);
        luaState.Dispose();
        luaState = null;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 240, 80), " = OnClick1"))
        {
            CallLuaFunction(setClick1);
        }
        else if (GUI.Button(new Rect(10, 90, 240, 80), " + Click1"))
        {
            CallLuaFunction(addClick1);
        }
        else if (GUI.Button(new Rect(10, 170, 240, 80), " + Click2"))
        {
            CallLuaFunction(addClick2);
        }
        else if (GUI.Button(new Rect(10, 260, 240, 80), " - Click1"))
        {
            CallLuaFunction(removeClick1);
        }
        else if (GUI.Button(new Rect(10, 350, 240, 80), " - Click2"))
        {
            CallLuaFunction(removeClick2);
        }
        else if (GUI.Button(new Rect(10, 440, 240, 80), "+ Click1 in C#"))
        {
            LuaFunction func = luaState.GetFunction("DoClick1");
            TestEventListener.OnClick onClick = (TestEventListener.OnClick)DelegateTraits<TestEventListener.OnClick>.Create(func);
            listener.onClick += onClick;
        }
        else if (GUI.Button(new Rect(10, 530, 240, 80), " - Click1 in C#"))
        {
            LuaFunction func = luaState.GetFunction("DoClick1");
            listener.onClick = (TestEventListener.OnClick)DelegateFactory.RemoveDelegate(listener.onClick, func);
            func.Dispose();
            func = null;
        }
        else if (GUI.Button(new Rect(10, 620, 240, 80), "OnClick"))
        {
            if (listener.onClick != null)
            {
                listener.onClick(gameObject);
            }
            else
            {
                Debug.Log("empty delegate!!");
            }
        }
        else if (GUI.Button(new Rect(10, 710, 240, 80), "Override"))
        {
            CallLuaFunction(testOverride);
        }
        else if (GUI.Button(new Rect(10, 810, 240, 80), "Force GC"))
        {
            //自动gc log: collect lua reference name , id xxx in thread 
            luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
            GC.Collect();
        }
        else if (GUI.Button(new Rect(10, 900, 240, 80), "event +"))
        {
            CallLuaFunction(addEvent);
        }
        else if (GUI.Button(new Rect(10, 990, 240, 80), "event -"))
        {
            CallLuaFunction(removeEvent);
        }
        else if (GUI.Button(new Rect(260, 10, 240, 80), "event call"))
        {
            listener.OnClickEvent(gameObject);
        }
        else if (GUI.Button(new Rect(260, 90, 240, 80), "+self call"))
        {
            CallLuaFunction(addSelfClick);
        }
        else if (GUI.Button(new Rect(260, 170, 240, 80), "-self call"))
        {
            CallLuaFunction(removeSelfClick);
        }
        else if (GUI.Button(new Rect(260, 250, 240, 80), "call onclick"))
        {
            CallLuaFunction(callOnClick);
            //listener.onAct();
        }
    }
}
