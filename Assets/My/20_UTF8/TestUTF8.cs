using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

/// <summary>
/// utf8也需要库
/// </summary>
public class TestUTF8 : LuaClient
{
    private const string script =
        @"
    local utf8 = utf8

    function Test()        
	    local l1 = utf8.len('你好')
        local l2 = utf8.len('こんにちは')
        print('chinese string len is: '..l1..' japanese sting len: '..l2)     

        local s = '遍历字符串'                                        

        for i in utf8.byte_indices(s) do            
            local next = utf8.next(s, i)                   
            print(s:sub(i, next and next -1))
        end   

        local s1 = '天下风云出我辈'        
        print(utf8.count('没有关系有关有关','有关'))
        print('风云 count is: '..utf8.count(s1, '风云'))
        s1 = s1:gsub('风云', '風雲')

        local function replace(s, i, j, repl_char)            
	        if s:sub(i, j) == '辈' then
		        return repl_char            
	        end
        end

        print(utf8.replace(s1, replace, '輩'))
    end
";


    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("Test");
        func.Call();
        func.Dispose();
        func = null;
    }


}
