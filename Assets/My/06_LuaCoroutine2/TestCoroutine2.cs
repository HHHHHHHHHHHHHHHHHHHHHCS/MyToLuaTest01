using UnityEngine;
using System.Collections;
using LuaInterface;
using UnityEngine.UI;

//两套协同勿交叉使用，类unity原生，大量使用效率低
public class TestCoroutine2 : LuaClient
{
    public Button startButton;
    public Button stopButton;

    private bool beStart = false;

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();

        startButton.onClick.AddListener(StartCor);
        stopButton.onClick.AddListener(StopCor);

        var scirpt = Resources.Load<TextAsset>("TestCoroutine2.lua");
        luaState.DoString(scirpt.text, "TestCoroutine2.cs");
        LuaFunction func = luaState.GetFunction("TestCo");
        func.Call();
        func.Dispose();
        func = null;
    }

    private void StartCor()
    {
        if (!beStart)
        {
            beStart = true;
            LuaFunction func = luaState.GetFunction("StartDelay");
            func.Call();
            func.Dispose();
        }
    }

    private void StopCor()
    {
        if (beStart)
        {
            beStart = false;
            LuaFunction func = luaState.GetFunction("StopDelay");
            func.Call();
            func.Dispose();
        }
    }

}
