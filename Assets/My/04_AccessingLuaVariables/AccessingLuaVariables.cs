using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UnityEngine;

public class AccessingLuaVariables : MonoBehaviour
{
    private void Start()
    {
        LuaState luaState = new LuaState();
        string fullPath = Path.Combine(Application.dataPath, "My/03_AccessingLuaVariables");
        luaState.AddSearchPath(fullPath);
        luaState.Require("AccessingLuaVariables");
        
    }
}
