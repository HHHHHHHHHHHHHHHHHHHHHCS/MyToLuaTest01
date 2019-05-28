using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class TestInherit : MonoBehaviour
{
    private const string script =
        @"
            LuaTransform={}

            function LuaTransform.Extend(u)
	            local t = {}
	            local _position = u.position
	            tolua.setpeer(u,t)

	            t.__index = t
	            local get = tolua.initget(t)
	            local set = tolua.initset(t)
	            
	            local _base = u.base

	            --重写同名属性获取
	            get.position = function(self)
		            return _position
	            end

	            --重写同名属性设置
	            set.position = function(self,v)
		            if _position ~= v then
			            _position = v
			            _base.position = v
		            end
	            end

	            --重写同名函数
	            function t:Translate(...)
		            print('Child Translate')
		            _base:Translate(...)
	            end

                return u
            end

            function Test(node)
	            local v = Vector3.one

	            local transform = LuaTransform.Extend(node)

	            local t = os.clock()
	            for i=1,200000 do
		            transform.position = transform.position
	            end
	            print('LuaTransform get set cost',os.clock()-t)

	            transform:Translate(1,1,1)

	            local child = transform:Find('child')
	            print('child is:',tostring(child))

	            if child.parent == transform then
		            print('LuaTransform compare to userdata transform is OK')
	            end

	            transform.xyz = 123
	            transform.xyz = 456
	            print('extern field xyz is:'..transform.xyz)
            end
            ";

    private LuaState luaState;

    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();
        LuaBinder.Bind(luaState);
        luaState.DoString(script,"TestInherit.cs");

        float time = Time.realtimeSinceStartup;

        for (int i = 0; i < 200000; i++)
        {
            Vector3 v = transform.position;
            transform.position = v;
        }

        time = Time.realtimeSinceStartup - time;

        Debugger.Log("C# transform get set cost time:"+time);
        luaState.Call("Test",transform,true);
        luaState.Dispose();
        luaState = null;

    }
}
