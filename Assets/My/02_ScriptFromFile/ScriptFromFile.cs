using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class ScriptFromFile : MonoBehaviour
{
    public Button doFileBtn;
    public Button requireBtn;
    public Text logText;

    private LuaState lua = null;

    private void Awake()
    {
        doFileBtn.onClick.AddListener(()=>LoadFile(true));
        requireBtn.onClick.AddListener(() => LoadFile(false));
        Application.logMessageReceived += Log;

    }

    private void Start()
    {
        lua= new LuaState();
        lua.Start();
        string fullPath = Application.dataPath + @"/My/02_ScriptFromFile";
        lua.AddSearchPath(fullPath);
    }

    private void Log(string condition, string stackTrace, LogType type)
    {
        logText.text += condition;
    }

    private void LoadFile(bool isDoFile)
    {
        logText.text = "";
        if (isDoFile)
        {
            lua.DoFile("ScriptsFromFile.lua");
        }
        else
        {
            lua.Require("ScriptsFromFile");
        }


        lua.Collect();
        lua.CheckTop();
    }


    private void OnDestroy()
    {
        lua.Dispose();
        lua = null;
        Application.logMessageReceived -= Log;
    }
}

