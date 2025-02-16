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
        isStopping = true;
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

    public Action<int> OnBalanceChanged;
    public void PlaceBet()
    {
        OnBalanceChanged?.Invoke(-newBetAmount);
        winCheckOnce = true;
        StartSpin();
    }

    public Action<int, int> OnBetWon;

    private void Update()
    {
        if (startSpinning || isStopping)
        {
            if (slotreel1.isSpinning == false && slotreel2.isSpinning == false && slotreel3.isSpinning == false)
            {
                StartCoroutine(WinStatus());
            }
        }
    }

    private bool winCheckOnce = false;
    IEnumerator WinStatus()
    {
        yield return new WaitForSeconds(1f);
        if (winCheckOnce)
        {
            isStopping = false;
            WinChecking();
        }
    }

    private int j;
    public void WinChecking()
    {
        winCheckOnce = false;
        startSpinning = false;
        Debug.Log("Win Checking "+j+" called this many times");
        j++;

        for (int i = 0; i < 3; i++)
        {
            if (slotreel1.rayCasterSymbol.Count > 0 && slotreel2.rayCasterSymbol.Count > 0 &&
                slotreel3.rayCasterSymbol.Count > 0)
            {
                if (slotreel1.rayCasterSymbol[i].symbolID == slotreel2.rayCasterSymbol[i].symbolID && slotreel1.rayCasterSymbol[i].symbolID == slotreel3.rayCasterSymbol[i].symbolID)
                {
                    if (i == 0)
                    {
                        OnBetWon?.Invoke(newBetAmount * 5, 1);
                    }
                    else if (i == 1)
                    {
                        OnBetWon?.Invoke(newBetAmount * 10, 2);
                    }
                    else
                    {
                        OnBetWon?.Invoke(newBetAmount * 20, 3);
                    }
                    
                    
                }
            }
        }
    }
    
}
