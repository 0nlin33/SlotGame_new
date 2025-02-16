using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Slotreel : MonoBehaviour
{
    public List<Symbols> reelSymbols;
    private Symbols targetSymbol;

    public RayCaster[] rayCasters;

    public int radius = 5;
    public int reelNumber = 0;
    
    
    public bool spawnReel = true;
    
    public bool isSpinning;
    public bool fastPlay;
    
    private SlotController slotController;
    
    public List<Symbols> reelSymbolsList = new List<Symbols>();

    public float stoppingAngle;
    private float decelerationDuration = 0.5f;
    
    private int generatedValue;



    public bool simulateOutcome;
    public int simulatedSymbolIndex;
    
    
    public Action<bool> OnSpinStop;
    
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
        slotController.OnFastModeToggled += ChangeFastMode;
        stoppingAngle = 360/reelSymbolsList.Count;
        
        fastPlay = slotController.fastPlay;
    }

    private void ChangeFastMode(bool obj)
    {
        fastPlay = obj;
    }

    public int generatedSymbolIndex;
    int SymbolIndex()
    {
        int stoppingSymbolIndex;
        if (simulateOutcome)
        {
            stoppingSymbolIndex = simulatedSymbolIndex;
        }
        else
        {
            stoppingSymbolIndex = Random.Range(0, reelSymbolsList.Count-1);
            generatedSymbolIndex = stoppingSymbolIndex;
        }
        
        return stoppingSymbolIndex;
    }

    private void SpinReel(bool obj)
    {
        if (obj)
        {
            generatedValue = SymbolIndex();
            SpinReel();
        }
        else
        {
            Debug.Log("SpinReel waiting to be pushed");
        }
    }


    private void StopReel(bool obj)
    {
        targetSymbol =  reelSymbolsList[generatedValue];

        if (targetSymbol == null)
        {
            Debug.LogError("No valid symbol found for stopping!");
            return;
        }

        // Snap to the exact angle where the symbol aligns with the middle raycaster
        float targetRotationX = targetSymbol.transform.localEulerAngles.x;

        transform.DORotate(new Vector3(-generatedValue*stoppingAngle-96, 0, 0), decelerationDuration, RotateMode.Fast)
            .SetEase(Ease.Linear).OnComplete(()=>ResultSymbols());

        spinTween.Kill();
        isSpinning = false;
    }

    public List<Symbols> rayCasterSymbol = new List<Symbols>();


    void ResultSymbols()
    {
        for (int i = 0; i < rayCasters.Length; i++)
        {
            rayCasterSymbol.Add(rayCasters[i].GetSymbol());
            Debug.Log("Currently "+rayCasters[i].name+" is interacting with "+rayCasters[i].GetSymbol()+"in reel number "+reelNumber);
        }
    }


    private Tween spinTween;

    void SpinReel()
    {
        if (rayCasterSymbol.Count > 0)
        {
            rayCasterSymbol.Clear();
        }
        
        isSpinning = true;
        
        if (fastPlay)
        { 
            spinTween = transform.DORotate(new Vector3(360*-generatedValue*10/*generatedValue*stoppingAngle*10*/, 0, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .OnComplete(() => StopReel(true));
        }
        else
        { 
            spinTween = transform.DORotate(new Vector3(360*-generatedValue*10/*generatedValue*stoppingAngle*10*/, 0, 0), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .OnComplete(() => StopReel(true));
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
            
            //spawnedSymbol.name += Random.Range(0, 100000).ToString(); Identifier to compare
            
            spawnedSymbol.transform.parent = transform;
            
            spawnedSymbol.transform.LookAt(transform.position);
            if (i >= 2 && i <= 7)
            {
                spawnedSymbol.transform.Rotate(0,0,180);

            }
            else
            {
                spawnedSymbol.transform.Rotate(0,0,0);
            }
            
            reelSymbolsList.Add(spawnedSymbol);
        }
    }
}