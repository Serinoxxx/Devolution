using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Maintains the count for all spores, also contains functions for checking if we have enough spores, and consuming the spores
//It is assumed that you will always check for available spores before consuming
public class SporeSystem : MonoBehaviour {

    [SerializeField] private int _redSporeCount;
    [SerializeField] private int _greenSporeCount;
    [SerializeField] private int _blueSporeCount;
    [SerializeField] private Text _redSporeText;
    [SerializeField] private Text _greenSporeText;
    [SerializeField] private Text _blueSporeText;

    //The pending counts keep track of spores that have been asked for but not consumed
    private int _pendingRedSpores;
    private int _pendingGreenSpores;
    private int _pendingBlueSpores;

    private void Start()
    {
        SetSporeText(); 
    }

    //Call this to check if there are enough of a certain colored spore
    public bool SporesAvailable(Enums.SporeColors sporeColor, int amount)
    {
        bool available = false;
        switch (sporeColor)
        {
            case Enums.SporeColors.red:
                if (amount + _pendingRedSpores <= _redSporeCount)
                {
                    available = true;
                    _pendingRedSpores = amount;
                }
                break;
            case Enums.SporeColors.green:
                if (amount + _pendingGreenSpores <= _greenSporeCount)
                {
                    available = true;
                    _pendingGreenSpores = amount;
                }
                break;
            case Enums.SporeColors.blue:
                if (amount + _pendingBlueSpores <= _blueSporeCount)
                {
                    available = true;
                    _pendingBlueSpores = amount;
                }
                break;
        }

        return available;
    }

    //Call this to use a certain colored spore
    public void ConsumeSpores(Enums.SporeColors sporeColor, int amount)
    {
        switch (sporeColor)
        {
            case Enums.SporeColors.red:
                _redSporeCount -= amount;
                //_redSporeText.text = "x" + _redSporeCount;
                break;
            case Enums.SporeColors.green:
                _greenSporeCount -= amount;
                //_greenSporeText.text = "x" + _greenSporeCount;
                break;
            case Enums.SporeColors.blue:
                _blueSporeCount -= amount;
                //_blueSporeText.text = "x" + _blueSporeCount;
                break;
        }

        SetSporeText();
        ResetPending();
    }

    //Call this to add a certain colored spore
    public void AddSpores(Enums.SporeColors sporeColor, int amount)
    {
        switch (sporeColor)
        {
            case Enums.SporeColors.red:
                _redSporeCount += amount;
                
                break;
            case Enums.SporeColors.green:
                _greenSporeCount += amount;
                
                break;
            case Enums.SporeColors.blue:
                _blueSporeCount += amount;
                
                break;
        }

        SetSporeText();
    }

    private void SetSporeText()
    {
        _blueSporeText.text = "x" + _blueSporeCount;
        _greenSporeText.text = "x" + _greenSporeCount;
        _redSporeText.text = "x" + _redSporeCount;
    }

    public void ResetPending()
    {
        _pendingRedSpores = 0;
        _pendingGreenSpores = 0;
        _pendingBlueSpores = 0;
    }


}
