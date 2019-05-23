using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class AccessingEnum : MonoBehaviour
{
    public Button changeBtn;

    private LuaState luaState;
    private int count = 1;

    private void Awake()
    {
        changeBtn.onClick.AddListener(ChangeType);
    }

    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();

        LuaBinder.Bind(luaState);

        var scirpt = Resources.Load<TextAsset>("AccessingEnum.lua");
        luaState.DoString(scirpt.text);
        luaState["space"] = Space.World;

        LuaFunction func = luaState.GetFunction("TestEnum");
        func.BeginPCall();
        func.Push(Space.World);
        func.PCall();
        func.EndPCall();
        func.Dispose();
        func = null;
    }

    private void ChangeType()
    {
        GameObject go = GameObject.Find("/Directional Light");
        Light light = go.GetComponent<Light>();
        LuaFunction func = luaState.GetFunction("ChangeLightType");
        func.BeginPCall();
        func.Push(light);
        LightType type = (LightType) (count++ % 4);
        func.Push(type);
        func.PCall();
        func.EndPCall();
        func.Dispose();
    }

    private void OnDestroy()
    {
        luaState.Dispose();
    }
}
