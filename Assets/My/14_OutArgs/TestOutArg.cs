using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using LuaInterface;
using UnityEngine;

public class TestOutArg : MonoBehaviour
{
    private const string script =
        @"
            local box = UnityEngine.BoxCollider

            function TestPick(ray)
                local _layer = LayerMask.GetMask('Default')
                local time = os.clock()
                local flag,hit = UnityEngine.Physics.Raycast(ray,nil,5000,_layer)
                --local flag,hit = UnityEngine.Physics.Raycast(ray,RaycastHit.out,5000,_layer)

                if flag then
                    print('pick from lua , point:'..tostring(hit.point))
                end
            end

        ";

    private Camera camera;

    private LuaState luaState;
    private LuaFunction func;

    private void Awake()
    {
        camera=Camera.main;

        luaState = new LuaState();
        LuaBinder.Bind(luaState);
        luaState.Start();

        luaState.DoString(script, "TestOutArg.cs");
        func = luaState.GetFunction("TestPick");

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            bool result = Physics.Raycast(ray, out var hit,5000, LayerMask.GetMask("Default"));

            if (result)
            {
                Debugger.Log($"check ray in C# gameObject : {hit.collider.name} {hit.point}");
            }

            func.BeginPCall();
            func.Push(ray);
            func.PCall();
            func.EndPCall();
        }

        luaState.CheckTop();
        luaState.Collect();
    }

    private void OnDestroy()
    {
        func.Dispose();
        func = null;
        luaState.Dispose();
        luaState = null;
    }
}
