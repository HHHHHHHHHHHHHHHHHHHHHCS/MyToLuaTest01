using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class TestString : MonoBehaviour
{
    private const string script =
        @"           
    function Test()
        local str = System.String.New('男儿当自强')
        local index = str:IndexOfAny('儿自')
        print('and index is: '..index)
        local buffer = str:ToCharArray()
        print('str type is: '..type(str)..' buffer[0] is ' .. buffer[0])
        local luastr = tolua.tolstring(buffer)
        print('lua string is: '..luastr..' type is: '..type(luastr))
        luastr = tolua.tolstring(str)
        print('lua string is: '..luastr)                    
    end
";

    private void Awake()
    {
        LuaState luaState = new LuaState();
        luaState.Start();
        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("Test");
        func.Call();
        func.Dispose();
        luaState.Dispose();
    }
}