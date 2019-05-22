using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public sealed class TestAccount
{
    public int id;
    public string name;
    public int sex;

    public TestAccount(int id, string name, int sex)
    {
        this.id = id;
        this.name = name;
        this.sex = sex;
    }
}


public class UseDictionary : MonoBehaviour
{
    public  Dictionary<int,TestAccount> accountDic = new Dictionary<int, TestAccount>();

    private void Awake()
    {
        accountDic.Add(1,new TestAccount(1,"AAA",1));
        accountDic.Add(2, new TestAccount(2, "BBB", 0));
        accountDic.Add(3, new TestAccount(3, "CCC", 1));

        LuaState luaState = new LuaState();
        luaState.Start();

        BindMap(luaState);

        var script = Resources.Load<TextAsset>("UseDictionary.lua");
        luaState.DoString(script.text,"UseDictionary.cs");

        LuaFunction func = luaState.GetFunction("TestDict");
        func.BeginPCall();
        func.Push(accountDic);
        func.PCall();
        func.EndPCall();

        func.Dispose();
        func = null;
        luaState.CheckTop();
        luaState.Dispose();
        luaState = null;


    }

    //示例方式，方便删除，正常导出无需手写下面代码
    void BindMap(LuaState L)
    {
        L.BeginModule(null);
        TestAccountWrap.Register(L);
        L.BeginModule("System");
        L.BeginModule("Collections");
        L.BeginModule("Generic");
        System_Collections_Generic_Dictionary_int_TestAccountWrap.Register(L);
        System_Collections_Generic_KeyValuePair_int_TestAccountWrap.Register(L);
        L.BeginModule("Dictionary");
        System_Collections_Generic_Dictionary_int_TestAccount_KeyCollectionWrap.Register(L);
        System_Collections_Generic_Dictionary_int_TestAccount_ValueCollectionWrap.Register(L);
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
    }
}
