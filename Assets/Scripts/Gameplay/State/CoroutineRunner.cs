using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public Coroutine Run(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}
