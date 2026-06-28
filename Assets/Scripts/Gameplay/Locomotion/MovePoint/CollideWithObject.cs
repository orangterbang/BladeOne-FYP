using UnityEngine;

public class CollideWithObject : MonoBehaviour
{
    [SerializeField]private bool targetInCurrPoint = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ActorContext>(out ActorContext targetActor))
        {
            targetInCurrPoint = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<ActorContext>(out ActorContext targetActor))
        {
            targetInCurrPoint = false;
        }
    }

    public Point isTargetInCurrentPoint() { 
            if(!targetInCurrPoint)return null;

            Point p = this.GetComponent<Point>();
            return p;
        }
}
