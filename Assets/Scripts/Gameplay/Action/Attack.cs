//This class will be light attack if i have time to add more
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Actor")]
    [SerializeField] private MovementPoint movementPoint;
    [SerializeField] private CombatReceiver combatReceiver;

    [Header("Target")]
    [SerializeField] private MovementPoint targetMovementPoint;//Holds the movement point of the target/enemy
    [SerializeField] private CombatReceiver targetCombatReceiver;

    [Header("Data")]
    [SerializeField] private float attackDamage = 10f;

    void Start()
    {
        combatReceiver = GetComponent<CombatReceiver>();
    }
    
    public void ExecuteAttack(Direction direction)
    {
        //NOTE: for light attack, direction is not used to get the point as it will always be the front point
        //Maybe will add a new script that tells the target to receive hit
        //this receive hit will check if the target is doing parry/dodge and the hit is within the timer for these action
        //if yes then something happen to this character
        //if not then target receive the hit and gets damage
        var attackActionPoint = movementPoint.GetPoint(Direction.Up);

        var currentPoint = movementPoint.TryGetPointWithTarget();
        var isAttackerinAttackPoint = movementPoint.ComparePoints(attackActionPoint, currentPoint);

        if(!isAttackerinAttackPoint) {Debug.Log("Attack Doesnt hit");return;}

        var attackPoint = targetMovementPoint.GetPoint(Direction.Up);
        
        var targetAttackPoint = targetMovementPoint.TryGetPointWithTarget();
        var isTargetinAttackPoint = targetMovementPoint.ComparePoints(attackPoint, targetAttackPoint);

        if (isTargetinAttackPoint)
        {
            //Receive Hit/send hit
            var attackReceived = targetCombatReceiver.ReceiveHit(attackDamage, direction);

            if (!attackReceived && targetCombatReceiver.DefensiveActionPerformed() == ActionEvent.ParryPressed)
            {
                combatReceiver.ActorParried();
            }
        }
    }
}
