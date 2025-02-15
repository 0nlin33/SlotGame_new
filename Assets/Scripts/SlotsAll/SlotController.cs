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
    
    private bool isSpinning = false;
    private bool isStopping = false;

    [Header("Actions")] 
    public Action<bool> OnStart;
    public Action<bool> OnStop;
        

    void Start()
    {
        
    }
    
    public void StartSpin()
    {
        isSpinning = true;
        OnStart?.Invoke(true);
    }
    
    public void StopSpin()
    {
        Debug.Log("Stop Spin pressed");
        isStopping = true;
        OnStop?.Invoke(true);
    }
}
