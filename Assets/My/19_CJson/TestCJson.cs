using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

/// <summary>
/// 因为需要json库 ,所以要继承LuaClient
/// </summary>
public class TestCJson : LuaClient
{
    private const string json = @"{
    ""glossary"": {
        ""title"": ""example glossary"",
                ""GlossDiv"": {
            ""title"": ""S"",
                        ""GlossList"": {
                ""GlossEntry"": {
                    ""ID"": ""SGML"",
                                        ""SortAs"": ""SGML"",
                                        ""GlossTerm"": ""Standard Generalized Mark up Language"",
                                        ""Acronym"": ""SGML"",
                                        ""Abbrev"": ""ISO 8879:1986"",
                                        ""GlossDef"": {
                        ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",
                                                ""GlossSeeAlso"": [""GML"", ""XML""]
                    },
                                        ""GlossSee"": ""markup""
                }
            }
        }
    }
}
";

    private const string script = @"
    local json = require 'cjson'

    function Test(str)
	    local data = json.decode(str)
        print(data.glossary.title)
	    s = json.encode(data)
	    print(s)
    end
";

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void OpenLibs()
    {
        base.OpenLibs();
        OpenCJson();
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();

        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("Test");
        func.BeginPCall();
        func.Push(json);
        func.PCall();
        func.EndPCall();
        func.Dispose();
    }

    new void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}