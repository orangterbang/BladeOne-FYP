using UnityEngine;

public class Point : MonoBehaviour
{
    public Direction pointDirection;
    public Vector3 pointPos{get; private set;}
    public CollideWithObject collideWithObject {get; private set;}
    void Awake()
    {
        collideWithObject = GetComponent<CollideWithObject>();
        pointPos = transform.position;
    }
}
