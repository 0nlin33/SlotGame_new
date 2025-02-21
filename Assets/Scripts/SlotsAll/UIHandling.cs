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

        
        balanceAmountText.text = slotController.BalanceInfo().ToString();
        betAmountText.text = slotController.currentBetAmount().ToString();
        winAmountText.text = " ";
        
        spinButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        
        FastModeOn.gameObject.SetActive(true);
        FastModeOff.gameObject.SetActive(false);
    }
    
    private bool wonMessage = false;

    private void OnBetWon(int winAmount, int winLine)
    {
        spinButton.interactable = false;
        wonMessage = true;
        winAmountText.text = winAmount.ToString();
        messageDisplayText.text = "Congratulations, You win "+winAmount;
        
        StartCoroutine(DisplayWinLine(winLine));
    }

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
        
        StartCoroutine(EnableSpinButton());
    }

    IEnumerator EnableSpinButton()
    {
        yield return new WaitForSeconds(1f);
        spinButton.interactable = true;
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
        spinButton.interactable = true;

        if (!wonMessage)
        {
            messageDisplayText.text = "Press Spin to play!!!";
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
