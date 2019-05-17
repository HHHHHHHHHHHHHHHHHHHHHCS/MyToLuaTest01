using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UnityEngine;

public class AccessingLuaVariables : MonoBehaviour
{
    private LuaState luaState;

    private void Start()
    {
        luaState = new LuaState();
        luaState.Start();
        string fullPath = Path.Combine(Application.dataPath, "My/04_AccessingLuaVariables");
        luaState.AddSearchPath(fullPath);

        luaState["Objs2Spawn"] = 5;

        luaState.Require("AccessingLuaVariables");

        Debugger.Log("Read var from lua:{0}",luaState["var2Read"]);
        Debugger.Log("Read table var from lua:{0}",luaState["varTable.default"]);

        LuaFunction func = luaState["TestFunc"] as LuaFunction;
        func.Call();
        func.Dispose();

        LuaTable table = luaState.GetTable("varTable");
        Debugger.Log("Read varTable from lua, default : {0} name:{1}",table["default"],table["name"]);

        table["map.name"] = "new";
        Debugger.Log("Modify varTable name:{0}",table["map.name"]);
        LuaFunction printName = luaState.GetFunction("PrintName");
        printName.Call();

        table.AddTable("newmap");
        LuaTable table1 = (LuaTable)table["newmap"];
        table1["name"] = "table1";
        Debugger.Log("varTable.newmap name:{0}", table1["name"]);
        table1.Dispose();

        table1 = table.GetMetaTable();

        if (table1 != null)
        {
            Debugger.Log("varTable metatable name:{0}", table1["name"]);
        }

        object[] list = table.ToArray();

        print("==============================================");
        for (int i = 0;  i < list.Length;  i++)
        {
            Debugger.Log("varTable[{0}], is {1}",i,list[i]);
        }
        
        table.Dispose();
    }


    private void OnDestroy()
    {
        luaState.Dispose();
        luaState = null;
    }
}
