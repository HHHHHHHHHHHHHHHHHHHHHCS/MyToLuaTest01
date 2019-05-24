using UnityEngine;
using System.IO;
using LuaInterface;

//use menu Lua->Copy lua files to Resources. 之后才能发布到手机
public class TestCustomLoader : LuaClient
{
    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void StartMain()
    {
        luaState.DoFile("TestLoader.lua");
        CallMain();
    }

    protected override void CallMain()
    {
        LuaFunction func = luaState.GetFunction("Test");
        func.Call();
        func.Dispose();
    }


    new void Awake()
    {
        base.Awake();
    }

    new void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

}
