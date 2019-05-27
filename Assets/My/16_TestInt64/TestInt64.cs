using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class TestInt64 : MonoBehaviour
{
    private const string script =
        @"            
            function TestInt64(x)                
                x = 789 + x
                assert(tostring(x) == '9223372036854775807')		                                       
                local low, high = int64.tonum2(x)                
                print('x value is: '..tostring(x)..' low is: '.. low .. ' high is: '..high.. ' type is: '.. tolua.typename(x))           
                local y = int64.new(1,2)                
                local z = int64.new(1,2)
                
                if y == z then
                    print('int64 equals is ok, value: '..int64.tostring(y))
                end

                x = int64.new(123)                   
            
                if int64.equals(x, 123) then
                    print('int64 equals to number ok')
                else
                    print('int64 equals to number failed')
                end

                x = int64.new('78962871035984074')
                print('int64 is: '..tostring(x))

                local str = tostring(int64.new(3605690779, 30459971))                
                local n2 = int64.new(str)
                local l, h = int64.tonum2(n2)                        
                print(str..':'..tostring(n2)..' low:'..l..' high:'..h)                  

                print('----------------------------uint64-----------------------------')
                x = uint64.new('18446744073709551615')                                
                print('uint64 max is: '..tostring(x))
                l, h = uint64.tonum2(x)      
                str = tostring(uint64.new(l, h))
                print(str..':'..tostring(x)..' low:'..l..' high:'..h)     

                return y
            end
        ";

    private void Start()
    {
        LuaState luaState = new LuaState();
        luaState.Start();
        luaState.DoString(script,"TestInt64.cs");

        LuaFunction func = luaState.GetFunction("TestInt64");
        func.BeginPCall();
        func.Push(9223372036854775807 - 789);
        func.PCall();
        long n64 = func.CheckLong();
        Debugger.Log("int64 return from lua is: {0}", n64);
        func.EndPCall();
        func.Dispose();
        func = null;

        luaState.CheckTop();
        luaState.Dispose();
        luaState = null;

    }
}
