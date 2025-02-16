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

        slotController.OnBetPlaced += ChangeCurrentBalance;
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

    private void OnBetWon(int winAmount)
    {
        winAmountText.text = winAmount.ToString();
        messageDisplayText.text = "Congratulations, You win "+winAmount;
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
        
        spinButton.interactable = true;
    }

    private void StartPressed(bool obj)
    {
        spinButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        
        messageDisplayText.text = "Good Luck!";

        StartCoroutine(ShowSpinButton());
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
        
        messageDisplayText.text = "Press Spin to play!!!";
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
