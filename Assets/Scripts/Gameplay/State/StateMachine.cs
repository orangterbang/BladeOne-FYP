//Script Reference and Taken from https://medium.com/dotcrossdot/hierarchical-finite-state-machine-c9e3f4ce0d9e
//May add try, catch & throw here just incase
//May need to add MonoBehavior incase there's a problem when other class derived from this
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachine
{
    private StateMachine currentSubState;
    private StateMachine defaultSubState;
    private StateMachine parent;

    private Dictionary<Type, StateMachine> subStates = new Dictionary<Type, StateMachine>();
    private Dictionary<ActionInput, StateMachine> transitions = new Dictionary<ActionInput, StateMachine>();

    protected Direction direction;

    public void EnterStateMachine()
    {
        OnEnter();
        if(currentSubState == null && defaultSubState != null)
        {
            currentSubState = defaultSubState;
        }

        currentSubState?.EnterStateMachine();
    }

    public void UpdateStateMachine()
    {
        currentSubState?.UpdateStateMachine();
        OnUpdate();
    }

    public void ExitStateMachine()
    {
        currentSubState?.ExitStateMachine();
        OnExit();
    }

    protected virtual void OnEnter(){}
    protected virtual void OnUpdate(){}
    protected virtual void OnExit(){}

    public void LoadSubState(StateMachine subState)
    {
        if(subStates.Count == 0)
        {
            defaultSubState = subState;
        }

        subState.parent = this;

        subStates.Add(subState.GetType(), subState);
    }

    public void AddTransition(StateMachine from, StateMachine to, ActionInput trigger)
    {
        if(!subStates.TryGetValue(from.GetType(), out _))
        {
            Debug.Log($"State {GetType()} does not have a substate of type {from.GetType()} to transition from.");
        }

        if(!subStates.TryGetValue(to.GetType(), out _))
        {
            Debug.Log($"State {GetType()} does not have a substate of type {to.GetType()} to transition from.");
        }

        from.transitions.Add(trigger, to);
    }

    public void SendTrigger(ActionInput trigger)
    {
        var root = this;
        while(root?.parent != null)
        {
            root = root.parent;
        }

        while(root != null)
        {
            if(root.transitions.TryGetValue(trigger, out StateMachine toState))
            {
                root.parent?.ChangeSubState(toState);
                return;
            }

            root = root.currentSubState;
        }
    }

    public void SetDirection(Direction dir)
    {
        direction = dir;
    }

    private void ChangeSubState(StateMachine state)
    {
        currentSubState?.ExitStateMachine();
        //var newState = subStates[GetType()];
        if (!subStates.TryGetValue(state.GetType(), out var newState))
        {
            Debug.LogError($"State {state.GetType()} not registered in HFSM");
            return;
        }
        currentSubState = newState;
        newState.EnterStateMachine();
    }
}
