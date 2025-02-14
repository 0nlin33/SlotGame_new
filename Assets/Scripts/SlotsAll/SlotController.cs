using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    private static SlotController Instance;

    public static SlotController instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<SlotController>();

                if (Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.AddComponent<SlotController>();
                }
                
            }
            else
            {
                return Instance;
            }
            return Instance;
        }
    }

    [SerializeField]private Slotreel slotreel1;
    [SerializeField]private Slotreel slotreel2;
    [SerializeField]private Slotreel slotreel3;

    [SerializeField] private float rotationSpeed = 20f;

    [SerializeField] private RayCaster raycaster1;
    [SerializeField] private RayCaster raycaster2;
    [SerializeField] private RayCaster raycaster3;
    
    
    [Header("Spin Settings")]
    public float spinSpeed = 1000f; // Initial speed of rotation
    public float deceleration = 500f; // How quickly the reel slows down
    public float stopThreshold = 5f; // How close it needs to be to snap
    public List<Transform> symbolPositions = new List<Transform>(); // List of valid stop positions
    
    private bool isSpinning = false;
    private bool isStopping = false;
    private float currentSpeed;
    private Transform targetStopPosition;

    [Header("Actions")] public Action<bool> OnStop;

    void Awake()
    {
        
    }
        

    void Start()
    {
        currentSpeed = spinSpeed;
        for (int i = 0; i < slotreel1.reelSymbols.Count ; i++)
        {
            symbolPositions.Add(slotreel1.reelSymbolsList[i].transform);
        }
    }
    
    void Update()
    {
        Debug.Log("This is the current symbol is raycaster 1: "+raycaster1.GetSymbol());
        Debug.Log("This is the current symbol is raycaster 2: "+raycaster2.GetSymbol());
        Debug.Log("This is the current symbol is raycaster 3: "+raycaster3.GetSymbol());
        
        isStopping = !slotreel1.isSpinning;

        if (isSpinning)
        {
            slotreel1.transform.Rotate(Vector3.left * currentSpeed * Time.deltaTime);
            
            if (isStopping)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed <= stopThreshold)
                {
                    SnapToClosestSymbol();
                }
            }
        }
    }
    
    public void StartSpin()
    {
        isSpinning = true;
        isStopping = false;
        currentSpeed = spinSpeed;
    }
    
    public void StopSpin()
    {
        Debug.Log("Stop Spin pressed");
        isStopping = true;
        //OnStop?.Invoke(false);
        targetStopPosition = FindClosestSymbol();
    }
    
    private Transform FindClosestSymbol()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
    
        foreach (Transform symbol in symbolPositions)
        {
            float distance = Mathf.Abs(raycaster3.transform.eulerAngles.y - symbol.eulerAngles.y);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = symbol;
            }
        }
        return closest;
    }
    
    
    private void SnapToClosestSymbol()
    {
        if (targetStopPosition != null)
        {
            transform.rotation = Quaternion.Euler(0, targetStopPosition.eulerAngles.y, 0);
        }
            
        isSpinning = false;
        isStopping = false;
    }
}
