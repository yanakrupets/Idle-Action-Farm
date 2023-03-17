using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private TMP_Text _blocksText;
    [SerializeField] private Scrollbar _scrollbar;

    void Awake()
    {
        EventManager.StartListening(GameEvent.BLOCK_TO_STACK, OnAddBlockToStack);
        EventManager.StartListening(GameEvent.ADD_MONEY, OnAddMoney);
    }

    void OnDestroy()
    {
        EventManager.StopListening(GameEvent.BLOCK_TO_STACK, OnAddBlockToStack);
        EventManager.StopListening(GameEvent.ADD_MONEY, OnAddMoney);
    }

    private void OnAddBlockToStack(int blockInStackCount, int maxBlockCount)
    {
        _blocksText.text = blockInStackCount + "/" + maxBlockCount;
        _scrollbar.size = (float)blockInStackCount / maxBlockCount;
    }

    private void OnAddMoney(int coinsToAdd, int maxBlockCount)
    {
        StartCoroutine(Coins(coinsToAdd));

        _blocksText.text = 0 + "/" + maxBlockCount;
        _scrollbar.size = 0;
    }

    IEnumerator Coins(int coins)
    {
        var number = Convert.ToInt32(_coinsText.text);
        var time = coins == 0 ? 0.5f : (float)1 / coins;
        for (var i = 1; i <= coins; i++)
        {
            number++;
            _coinsText.text = number.ToString();

            yield return new WaitForSeconds(time);
        }
    }
}
