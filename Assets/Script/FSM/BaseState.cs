using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected List<IStateModule> modules;

    public BaseState(string name)
    {
        this.name = name;
        this.modules = new List<IStateModule>();
    }

    public virtual void Enter() {
        foreach (IStateModule module in modules) module.Enter();
    }

    public virtual void UpdateLogic()
    {
        foreach (IStateModule module in modules) module.UpdateLogic();
    }
    public virtual void UpdatePhysics()
    {
        foreach (IStateModule module in modules) module.UpdatePhysics();
    }
    public virtual void Exit() {
        foreach (IStateModule module in modules) module.Exit();
    }
}