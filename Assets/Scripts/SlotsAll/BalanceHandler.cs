using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceHandler : MonoBehaviour
{
    private int startingBalance = 25000;

    private int currentBalance;
    
    SlotController slotController;

    private void Awake()
    {
        currentBalance = startingBalance;
        
        slotController = SlotController.instance;

        slotController.OnBetPlaced += BalanceChange;
        slotController.OnBetWon += BalanceChange;
    }

    private void BalanceChange(int changeAmount)
    {
        currentBalance += changeAmount;
    }

    public int BalanceCheck()
    {
        return currentBalance;
    }
    
    public 
    void Update()
    {
        
    }
}
