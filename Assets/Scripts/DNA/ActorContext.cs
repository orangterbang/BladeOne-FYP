using UnityEngine;

public class ActorContext : MonoBehaviour
{
    public Movement movement { get; private set; }
    public Attack attack {get; private set; }
    public Parry parry { get; private set; }
    public AnimatorController animator { get; private set; }
    public CoroutineRunner coroutineRun { get; private set; }
    public ActorCombatData actorCombatData { get; private set; }
    
    protected virtual void Awake()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<AnimatorController>();
        attack = GetComponent<Attack>();
        parry = GetComponent<Parry>();
        coroutineRun = GetComponent<CoroutineRunner>();
        actorCombatData = GetComponent<ActorCombatData>();
    }
}
