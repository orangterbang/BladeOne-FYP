using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;
using Unity.VisualScripting;

public class VFXManager : MonoBehaviour
{
    // Static instance so ANY script can access it instantly
    public static VFXManager Instance { get; private set; }

    [Header("VFX Prefabs")]
    [SerializeField] private GameObject hitVFXPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10;

    // Storage room for our dormant effects
    private Queue<GameObject> hitPool = new Queue<GameObject>();

    void Awake()
    {
        // Simple Singleton pattern
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        InitializePool();
    }

    // Pre-warm the pool at game start
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(hitVFXPrefab, transform);
            obj.SetActive(false); // Hide them initially
            hitPool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Static shortcut! Any script can call VFXManager.PlayHit(position, rotation);
    /// </summary>
    /// 
    public static void PlayHit()
    {
        if (Instance == null)
        {
            Debug.LogError("VFXManager is missing from the scene!");
            return;
        }

        // Pull the oldest effect out of the pool
        GameObject vfxObj = Instance.hitPool.Dequeue();

        // Position it exactly where the hit happened
        vfxObj.transform.position = Instance.gameObject.transform.position;
        vfxObj.transform.rotation = Instance.gameObject.transform.rotation;
        vfxObj.SetActive(true);

        // Get the VFX component, reset its timeline, and play
        VisualEffect vfx = vfxObj.GetComponent<VisualEffect>();
        vfx.Reinit();
        vfx.Play();

        // Put it back at the end of the line so it can be reused later
        Instance.hitPool.Enqueue(vfxObj);
    }

    public static void PlayHit(Vector3 position, Quaternion rotation)
    {
        if (Instance == null)
        {
            Debug.LogError("VFXManager is missing from the scene!");
            return;
        }

        // Pull the oldest effect out of the pool
        GameObject vfxObj = Instance.hitPool.Dequeue();

        // Position it exactly where the hit happened
        vfxObj.transform.position = position;
        vfxObj.transform.rotation = rotation;
        vfxObj.SetActive(true);

        // Get the VFX component, reset its timeline, and play
        VisualEffect vfx = vfxObj.GetComponent<VisualEffect>();
        vfx.Reinit();
        vfx.Play();

        // Put it back at the end of the line so it can be reused later
        Instance.hitPool.Enqueue(vfxObj);
    }
}