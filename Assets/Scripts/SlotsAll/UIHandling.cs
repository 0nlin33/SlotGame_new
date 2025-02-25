using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button spinButton;
    [SerializeField] private Button stopButton;
    /*[SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;*/
    [SerializeField] private Button FastModeOn;
    [SerializeField] private Button FastModeOff;
    
    
    [Header("Graphic elements")]
    [SerializeField] private GameObject winLine1Effect;
    [SerializeField] private GameObject winLine2Effect;
    [SerializeField] private GameObject winLine3Effect;
    
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI betAmountText;
    [SerializeField] private TextMeshProUGUI balanceAmountText;
    [SerializeField] private TextMeshProUGUI winAmountText;
    [SerializeField] private TextMeshProUGUI messageDisplayText;
    
    [Header("Simulator Part for FreeSpin")]
    [SerializeField]private Slotreel slotreel1;
    [SerializeField]private Slotreel slotreel2;
    [SerializeField]private Slotreel slotreel3;
    [SerializeField] private Button enableSimulateButton;
    [SerializeField] private Button disableSimulateButton;

    SlotController slotController;
    // Start is called before the first frame update
    void Start()
    {
        slotController = SlotController.instance;

        slotController.OnBalanceChanged += ChangeCurrentBalance;
        slotController.OnStart += StartPressed;
        slotController.OnStop += StopPressed;
        slotController.OnFastModeToggled += ToggleFastMode;
        slotController.OnBetWon += OnBetWon;
        slotController.OnFreespinHit += FreeSpinDisplay;

        slotreel1.OnReelStop += OnReel1Stop;
        slotreel2.OnReelStop += OnReel2Stop;
        slotreel3.OnReelStop += OnReel3Stop;
        
        balanceAmountText.text = slotController.BalanceInfo().ToString();
        betAmountText.text = slotController.currentBetAmount().ToString();
        winAmountText.text = " ";
        
        spinButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        
        FastModeOn.gameObject.SetActive(true);
        FastModeOff.gameObject.SetActive(false);
        
        enableSimulateButton.gameObject.SetActive(true);
        disableSimulateButton.gameObject.SetActive(false);
    }

    bool reel1Stopped = false;
    bool reel2Stopped = false;
    bool reel3Stopped = false;
    void OnReel1Stop()
    {
        reel1Stopped = true;
    }

    void OnReel2Stop()
    {
        reel2Stopped = true;
    }

    void OnReel3Stop()
    {
        reel3Stopped = true;
        freeSpinRewardAmount -= freeSpinRewardAmount;
        if (freeSpinRewardAmount >= 0)
        {
            FreeSpinDisplay(freeSpinRewardAmount);
        }
        freeSpinCountText.gameObject.SetActive(false);
    }
    
    private bool wonMessage = false;

    public bool playerWon = false;
    public bool spinButtonStatus => spinButton.interactable;
    

    private void OnBetWon(int winAmount, int winLine)
    {
        playerWon = true;
        spinButton.interactable = false;
        wonMessage = true;
        winAmountText.text = winAmount.ToString();
        messageDisplayText.text = "Congratulations, You win "+winAmount;
        
        StartCoroutine(DisplayWinLine(winLine));
    }


    #region FreeSpinSimulator
    public void SimulateFreeSpinTrigger()
    {
        slotreel1.simulateOutcome = true;
        slotreel2.simulateOutcome = true;
        slotreel3.simulateOutcome = true;

        slotreel1.simulatedSymbolIndex = 5;
        slotreel2.simulatedSymbolIndex = 5;
        slotreel3.simulatedSymbolIndex = 5;
        
        enableSimulateButton.gameObject.SetActive(false);
        disableSimulateButton.gameObject.SetActive(true);
    }

    public void DisableFreeSpinSimulator()
    {
        slotreel1.simulateOutcome = false;
        slotreel2.simulateOutcome = false;
        slotreel3.simulateOutcome = false;
        
        
        enableSimulateButton.gameObject.SetActive(true);
        disableSimulateButton.gameObject.SetActive(false);
    }

    [SerializeField] private TextMeshProUGUI freeSpinCountText;
    [SerializeField]private int freeSpinRewardAmount = 0;
    private void FreeSpinDisplay(int freeSpinCount)
    {
        DisableFreeSpinSimulator();
        freeSpinCountText.gameObject.SetActive(true);
        freeSpinRewardAmount = freeSpinCount;
        freeSpinCountText.text = freeSpinCount.ToString();
    }
    #endregion
    

    IEnumerator DisplayWinLine(int winLine)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        
            if (winLine == 1)
            {
                winLine1Effect.SetActive(true);
            }
        
            yield return new WaitForSeconds(0.5f);
        
            if (winLine == 2)
            {
                winLine2Effect.SetActive(true);
            }
        
            winLine1Effect.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            if (winLine == 3)
            {
                winLine3Effect.SetActive(true);
            }
        
            winLine2Effect.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        
            winLine3Effect.SetActive(false);
        }
        
        spinButton.interactable = true;
        playerWon = false;
    }
    
    private void ToggleFastMode(bool obj)
    {
        FastModeOff.gameObject.SetActive(obj);
        FastModeOn.gameObject.SetActive(!obj);

        if (obj)
        {
            messageDisplayText.text = "Fast Mode turned on";
        }
        else
        {
            messageDisplayText.text = "Fast Mode turned off";
        }
    }

    private void StopPressed(bool obj)
    {
        stopButton.gameObject.SetActive(false);
        spinButton.gameObject.SetActive(true);
        
        StartCoroutine(EnableSpinButtonAfterStop());
    }

    IEnumerator EnableSpinButtonAfterStop()
    {
        yield return new WaitForSeconds(1f);
        if (!playerWon)
        {
            spinButton.interactable = true;
        }
    }

    private void StartPressed(bool obj)
    {
        spinButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);

        winAmountText.text = " ";
        
        if (!wonMessage)
        {
            messageDisplayText.text = "Good Luck!";
        }

        StartCoroutine(ShowSpinButton());

        balanceAmountText.text = slotController.BalanceInfo().ToString();
    }

    IEnumerator ShowSpinButton()
    {
        float stopTime = 3f;

        if (slotController.fastPlay)
        {
            stopTime = 0.5f;
        }
        else
        {
            stopTime = 3f;
        }
        
        spinButton.interactable = false;
        yield return new WaitForSeconds(stopTime);
        StopPressed(true);
        yield return new WaitForSeconds(.5f);
        if (!playerWon)
        {
            messageDisplayText.text = "Press Spin to play!!!";
            spinButton.interactable = true;
            playerWon = false;
        }
    }

    private void ChangeCurrentBalance(int obj)
    {
        balanceAmountText.text = slotController.BalanceInfo().ToString();
    }


    public void IncreaseBetAmount()
    {
        slotController.IncreaseBetAmount();
        betAmountText.text = slotController.currentBetAmount().ToString();
    }

    public void DecreaseBetAmount()
    {
        slotController.DecreaseBetAmount();
        betAmountText.text = slotController.currentBetAmount().ToString();
    }
}
