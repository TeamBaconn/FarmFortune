
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected BaseState currentState;
    protected string defaultState = "IdleState";
    protected List<BaseState> stateList = new List<BaseState>();

    protected virtual void Start()
    {
        if(currentState == null) ResetState();
    }

    protected virtual void OnDestroy()
    {
        currentState?.Exit();
    }

    protected virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    protected virtual void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    protected void CreateState(BaseState state)
    {
        foreach(BaseState s in stateList) if(s.name == state.name)
            {
                stateList.Remove(s);
                break;
            }
        stateList.Add(state);
    }

    public bool IsStateName(string name)
    {
        return currentState != null && currentState.name.Equals(name);
    }

    protected BaseState GetState(string name)
    {
        if (name == null) return null;
        foreach (BaseState state in stateList) if (state.name == name) return state;
        return null;
    }

    public void ChangeState(string name)
    {
        BaseState targetState = GetState(name);
        if (targetState == null)
        {
            Debug.LogWarning($"Cannot find state {name}");
            return;
        }
        if (currentState == targetState) return;
        currentState?.Exit();

        currentState = targetState;
        currentState.Enter();
    }

    protected bool NextState()
    {
        if (currentState == null) return false;

        int index = stateList.IndexOf(currentState)+1;
        if (index >= stateList.Count) return false;

        ChangeState(stateList[index].name);

        return true;
    }

    public void ResetState()
    {
        if (stateList.Count == 0) return;
        ChangeState(defaultState);
    }
}