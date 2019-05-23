using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

//CustomSetting.cs add in customDelegateList
public sealed class TestEventListener : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);

    public delegate void OnClick(GameObject go);

    public OnClick onClick = delegate { };

    public Action onAct;

    public event OnClick onClickEvent = delegate { };

    public Func<bool> TestFunc = null;

    public void SetOnFinished(OnClick click)
    {
        Debugger.Log("SetOnFinished OnClick");
    }

    public void SetOnFinished(VoidDelegate click)
    {
        Debugger.Log("SetOnFinished VoidDelegate");
    }

    [NoToLua]
    public void OnClickEvent(GameObject go)
    {
        onClickEvent(go);
    }
}
