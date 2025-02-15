using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slotreel : MonoBehaviour
{
    public List<Symbols> reelSymbols;
    private Symbols targetSymbol;

    public RayCaster raycast1;
    public RayCaster raycast2;
    public RayCaster raycast3;

    public int radius = 5;
    public int reelNumber = 0;
    
    public bool spawnReel = true;
    public float spinSpeed = 2000f;
    public float currentSpeed;
    public float decelerationRate = 500f;
    public float stopThreshold = 5f;
    
    public bool isSpinning;
    public bool isStopping;
    
    private SlotController slotController;
    
    public List<Symbols> reelSymbolsList = new List<Symbols>();
    public Transform[] raycasters;
    public float raycastMaxDistance = 0.8f;
    
    // Start is called before the first frame update
    void Awake()
    {
        /*if (spawnReel)
        {
            SpawnReelSymbols();
        }*/
        SpawnReelSymbols();
    }

    private void Start()
    {
        slotController = SlotController.instance;

        slotController.OnStop += StopReel;
        slotController.OnStart += SpinReel;
    }

    private void SpinReel(bool obj)
    {
        isSpinning = obj;
        currentSpeed = spinSpeed;
    }

    private void StopReel(bool obj)
    {
        isStopping = obj;
        targetSymbol = FindClosestSymbol();
    }

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(Vector3.left * currentSpeed * Time.deltaTime);

            // If stopping, decelerate
            if (isStopping)
            {
                currentSpeed -= decelerationRate * Time.deltaTime;
                if (currentSpeed <= stopThreshold)
                {
                    SnapToClosestSymbol();
                }
            }
        }
    }

    void SpawnReelSymbols()
    {
        if (reelSymbols == null || reelSymbols.Count == 0)
        {
            Debug.LogError("No Reel Symbols Found");
        }
        
        int numberOfSymbols = reelSymbols.Count;
        float angleIncrease = 360 / numberOfSymbols;

        for (int i = 0; i < numberOfSymbols; i++)
        {
            
            float radians = i * angleIncrease * Mathf.Deg2Rad;

            Vector3 spawnPosition = transform.position + new Vector3(0, Mathf.Cos(radians), Mathf.Sin(radians)) * radius;
            
            Symbols spawnedSymbol = Instantiate(reelSymbols[i], spawnPosition, Quaternion.Euler(0,0,0));
            
            spawnedSymbol.transform.parent = transform;
            
            spawnedSymbol.transform.LookAt(transform.position);
            spawnedSymbol.transform.Rotate(0,0,0);
            
            reelSymbolsList.Add(spawnedSymbol);
        }
    }
    
    private Symbols FindClosestSymbol()
    {
        Symbols closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Symbols symbol in reelSymbolsList)
        {
            foreach (Transform raycaster in raycasters)
            {
                
                float distance = Vector3.Distance(symbol.transform.position, raycaster.position);
                if (distance < minDistance)
                {
                    Debug.Log("This is the closest symbol : "+symbol.symbolName+" to this raycaster : "+raycaster.name);
                    
                    minDistance = distance;
                    closest = symbol;
                    
                    Debug.Log("Closest Symbol: " + closest.symbolName);
                    
                }
            }
        }
        
        if (closest != null)
        {
            Debug.Log("Closest Symbol: " + closest.symbolName);
        }
        
        return closest;
    }

    private void SnapToClosestSymbol()
    {
        if (targetSymbol != null)
        {
            // Snap the reel to the correct angle
            //transform.rotation = Quaternion.Euler(targetSymbol.transform.localEulerAngles.x, 0, 0);
            
            Quaternion targetRotation = Quaternion.Euler(targetSymbol.transform.localEulerAngles.x, 0, 0);
            StartCoroutine(SmoothSnap(targetRotation));
        }
        isSpinning = false;
        isStopping = false;
    }
    
    private IEnumerator SmoothSnap(Quaternion targetRotation)
    {
        float duration = 0.3f; // Snap duration
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}