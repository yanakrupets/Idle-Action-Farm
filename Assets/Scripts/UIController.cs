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

    [SerializeField] private Camera _camera;

    [SerializeField] private RectTransform _uiparticle;
    private ParticleSystem _particleSystem;

    private bool _coinCoroutineIsStarted = false;
    private int _coins = 0;

    void Awake()
    {
        EventManager.StartListening(GameEvent.BLOCK_TO_STACK, OnAddBlockToStack);
        EventManager.StartListening(GameEvent.ADD_MONEY, OnAddMoney);
        EventManager.StartListening(GameEvent.START_PARTICLE, OnStartParticle);
    }

    void OnDestroy()
    {
        EventManager.StopListening(GameEvent.BLOCK_TO_STACK, OnAddBlockToStack);
        EventManager.StopListening(GameEvent.ADD_MONEY, OnAddMoney);
        EventManager.StopListening(GameEvent.START_PARTICLE, OnStartParticle);
    }

    private void Start()
    {
        _particleSystem = _uiparticle.GetComponentInChildren<ParticleSystem>();
    }

    private void OnAddBlockToStack(int blockInStackCount, int maxBlockCount)
    {
        _blocksText.text = blockInStackCount + "/" + maxBlockCount;
        _scrollbar.size = (float) blockInStackCount / maxBlockCount;
    }

    private void OnAddMoney(int coins, int maxBlockCount, float time)
    {
        _coins = coins;

        StartCoroutine(BarToZero(maxBlockCount, time));
    }

    private void OnStartParticle(Vector3 position, int blocksCount, float time)
    {
        StartCoinsParticle(position, blocksCount, time);
    }

    private void StartCoinsParticle(Vector3 position, int particleCount, float time)
    {
        var main = _particleSystem.main;

        main.maxParticles = particleCount;

        var screenPoint = _camera.WorldToScreenPoint(position);
        if (screenPoint.x < 0) screenPoint.x = 0;
        _uiparticle.transform.position = screenPoint;

        _particleSystem.gameObject.SetActive(true);
        _particleSystem.Play();
    }

    public void AttachedCoin()
    {
        if (!_coinCoroutineIsStarted)
        {
            _coinCoroutineIsStarted = true;
            StartCoroutine(Coins(_coins));
        }
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

        _coinCoroutineIsStarted = false;
    }

    IEnumerator BarToZero(int maxBlockCount, float time)
    {
        var number = Convert.ToInt32(_blocksText.text
            .Substring(0, _blocksText.text
            .IndexOf("/")));
        var delta = (float) 1 / maxBlockCount;
        while (_scrollbar.size > 0)
        {
            number--;
            if (number < 0) number = 0;
            _blocksText.text = number + "/" + maxBlockCount;

            _scrollbar.size -= delta;

            yield return new WaitForSeconds(time);
        }
    }
}
