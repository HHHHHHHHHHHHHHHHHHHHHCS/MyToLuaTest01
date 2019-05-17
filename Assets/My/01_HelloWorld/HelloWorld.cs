using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    private void Awake()
    {
        LuaState lua = new LuaState();
        lua.Start();
        string hello =
            @"
                print('Hello World')
            ";
        lua.DoString(hello, "Hi.cs");
        lua.CheckTop();
        lua.Dispose();
    }
}
