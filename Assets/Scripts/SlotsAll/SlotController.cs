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

    [Header("Script Refrences")]
    [SerializeField]private Slotreel slotreel1;
    [SerializeField]private Slotreel slotreel2;
    [SerializeField]private Slotreel slotreel3;
    [SerializeField]private BalanceHandler balanceHandler;
    
    private bool isStopping = false;
    public bool fastPlay = false;

    [Header("Actions")] 
    public Action<bool> OnStart;
    public Action<bool> OnStop;


    public Action<bool> OnFastModeToggled;
    public void FastPlayToggle()
    {
        fastPlay = !fastPlay;
        OnFastModeToggled?.Invoke(fastPlay);
    }
    
    bool startSpinning = false;
    
    private void StartSpin()
    {
        startSpinning = true;
        OnStart?.Invoke(true);
    }
    
    public void StopSpin()
    {
        startSpinning = false;
        OnStop?.Invoke(true);
    }

    private int betIncreamentAmount = 2;
    private int newBetAmount = 2;

    public int currentBetAmount()
    {
        return newBetAmount;
    }

    public int BalanceInfo()
    {
        return balanceHandler.BalanceCheck();
    }
    
    public int IncreaseBetAmount()
    {
        newBetAmount += betIncreamentAmount;

        if (newBetAmount >= 20)
        {
            newBetAmount = 20;
        }
        
        return newBetAmount;
    }

    public int DecreaseBetAmount()
    {
        newBetAmount -= betIncreamentAmount;

        if (newBetAmount <= 2)
        {
            newBetAmount = 2;
        }
        
        return newBetAmount;
    }

    public Action<int> OnBetPlaced;
    public void PlaceBet()
    {
        OnBetPlaced?.Invoke(-newBetAmount);
        
        StartSpin();
    }

    public Action<int> OnBetWon;

    private void Update()
    {
        if (startSpinning)
        {
            if (slotreel1.isSpinning == false && slotreel2.isSpinning == false && slotreel3.isSpinning == false)
            {
                WinChecking();
            }
        }
        
    }

    private int i;
    public void WinChecking()
    {
        Debug.Log("Win Checking "+i+" called this many times");
        i++;
        if (slotreel1.rayCasters[0].GetSymbol() == slotreel2.rayCasters[0].GetSymbol() ==
            slotreel3.rayCasters[0].GetSymbol())
        {
            OnBetWon?.Invoke(newBetAmount * 5);
        }

        if (slotreel1.rayCasters[1].GetSymbol() == slotreel2.rayCasters[1].GetSymbol() ==
            slotreel3.rayCasters[1].GetSymbol())
        {
            OnBetWon?.Invoke(newBetAmount * 10);
        }
        
        if (slotreel1.rayCasters[2].GetSymbol() == slotreel2.rayCasters[2].GetSymbol() ==
            slotreel3.rayCasters[2].GetSymbol())
        {
            OnBetWon?.Invoke(newBetAmount * 20);
        }
        
        startSpinning = false;
        
    }
    
}
