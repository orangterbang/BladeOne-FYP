using UnityEngine;

public class Point : MonoBehaviour
{
    public Direction pointDirection;
    public Vector3 pointPos{get; private set;}
    void Start()
    {
        pointPos = transform.position;
    }
}
