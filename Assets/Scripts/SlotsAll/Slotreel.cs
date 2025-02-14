using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slotreel : MonoBehaviour
{
    public List<Symbols> reelSymbols;

    public int radius = 5;
    public int reelNumber = 0;
    
    public bool spawnReel = true;
    public float spinSpeed = 2000f;
    public bool isSpinning;
    
    private SlotController slotController;
    
    public List<Symbols> reelSymbolsList = new List<Symbols>();
    
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

        slotController.OnStop += StopSpinning;
    }

    private void StopSpinning(bool spinStatus)
    {
        isSpinning = spinStatus;
    }

    private void Update()
    {
        if (isSpinning)
        {
            RotateObjects();
        }
    }
    
    public void StartSpinning()
    {
        isSpinning = true;
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
    
    void RotateObjects()
    {
        transform.Rotate(Vector3.left * spinSpeed);
    }
}