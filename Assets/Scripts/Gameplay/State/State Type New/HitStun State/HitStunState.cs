using UnityEngine;

public class HitStunState : StateMachine
{
    ActorContext actor;
    public HitStunState(ActorContext actorContext)
    {
        this.actor=actorContext;

        StaggerState staggerState = new StaggerState(actor);
        StunnedState stunnedState = new StunnedState(actor);
        WakeUpState wakeUpState = new WakeUpState(actor);

        LoadSubState(staggerState);
        LoadSubState(stunnedState);
        LoadSubState(wakeUpState);

        AddTransition(staggerState, stunnedState, ActionEvent.ChangeSubState);
        AddTransition(stunnedState, wakeUpState, ActionEvent.ChangeSubState);
    }
}
