using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementPoint : MonoBehaviour
{
    //public GameObject actor;
    [SerializeField]private List<Point> points;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPoints();
    }

    void SetPoints()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            points.Add(child.GetComponent<Point>());
        }
    }

    public bool CheckPoint(Direction moveDirection)
    {
        return points.Any(p => p.pointDirection == moveDirection);
    }

    public Point GetPoint(Direction moveDirection)
    {
         return points.SingleOrDefault(p => p.pointDirection == moveDirection);
    }
}
