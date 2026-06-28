using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private MovementPoint movementPoint;//holds the movement point of itself
    [SerializeField] private float parryDuration = 0.25f;
    private CombatReceiver combatReceiver;

    void Start()
    {
        combatReceiver = GetComponent<CombatReceiver>();
    }

    public void ExecuteParry(Direction direction)
    {
        combatReceiver.ParryPerformed(direction);
    }

    public void ParryExecuted()
    {
        combatReceiver.ParryFinished();
    }

    public float GetParryDuration() => parryDuration;
}
