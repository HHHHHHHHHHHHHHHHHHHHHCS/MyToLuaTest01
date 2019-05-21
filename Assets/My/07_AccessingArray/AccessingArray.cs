using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LuaInterface;
using UnityEngine;

public class AccessingArray : MonoBehaviour
{
    private LuaState luaState;
    private LuaFunction func;

    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();

        //var loader = new LuaResLoader();//会有几个默认的读取文件夹 
        //loader.AddSearchPath(Application.dataPath+@"/My/07_AccessingArray/?.lua");//格式: 路径/?.lua
        //var luaScript = loader.FindFile("AccessingArray");

        TextAsset luaScript = Resources.Load<TextAsset>("AccessingArray.lua");
        luaState.DoString(luaScript.text);
        

        int[] array = {1, 2, 3, 4, 5};
        func = luaState.GetFunction("TestArray");

        func.BeginPCall();
        func.Push(array);
        func.PCall();
        double arg1 = func.CheckNumber();
        string arg2 = func.CheckString();
        bool arg3 = func.CheckBoolean();
        Debugger.Log($"return is {arg1} {arg2} {arg3}");
        func.EndPCall();


        //调用通用函数需要转换一下类型，避免可变参数拆成多个参数传递
        //慎用有GC
        object[] objs = func.LazyCall((object)array);

        if (objs != null)
        {
            Debugger.Log("return is {0} {1} {2}", objs[0], objs[1], objs[2]);
        }

        luaState.CheckTop();
    }

    private void OnDestroy()
    {
        func.Dispose();
        luaState.Dispose();
    }
}
