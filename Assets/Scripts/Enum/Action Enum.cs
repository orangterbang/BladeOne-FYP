public enum ActionInput{
    Move,
    Attack,
    Parry,
    Block,
    Stance
}

public enum ActionEvent
{
    OnHealthZero,
    OnRespawn,
    AttackPressed,
    ParryPressed,
    DodgePressed,
    CombatSequenceComplete,
    OnHitReceived,
    OnCritHitReceived,
    OnStunned,
    HitStunRecovered,
    ChangeSubState
}