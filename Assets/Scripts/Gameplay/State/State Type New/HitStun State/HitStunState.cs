using UnityEngine;

public class HitStunState : StateMachine
{
    ActorContext actor;
    public HitStunState(ActorContext actorContext)
    {
        this.actor=actorContext;

        StaggerState staggerState = new StaggerState(actor);
        CriticalStaggerState criticalStaggerState = new CriticalStaggerState(actor);
        StunnedState stunnedState = new StunnedState(actor);
        WakeUpState wakeUpState = new WakeUpState(actor);

        LoadSubState(staggerState);
        LoadSubState(criticalStaggerState);
        LoadSubState(stunnedState);
        LoadSubState(wakeUpState);

        AddTransition(staggerState, stunnedState, ActionEvent.OnStunned);
        AddTransition(stunnedState, wakeUpState, ActionEvent.ChangeSubState);
        AddTransition(stunnedState, criticalStaggerState, ActionEvent.OnCritHitReceived);
    }
}
