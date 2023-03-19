using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] private int _maxBlockCount = 40;

    [SerializeField] private float spaceBetweenX = 0.01f;
    [SerializeField] private float spaceBetweenZ = 0.06f;

    [SerializeField] private float deltaX = 0.3f;
    [SerializeField] private float deltaZ = 0.2f;
    [SerializeField] private float deltaY = 0.18f;

    [SerializeField] private float timeForMovingToBarn = 10f;

    private float _height;
    private List<Block> _blocksInStack;
    private Dictionary<Vector3, bool> _slots;
    public bool IsBlocksMoving { get; set; } = false;

    public int Coins { get; private set; }

    void Start()
    {
        _blocksInStack = new List<Block>();
        _slots = GetSlotsForLevel(_height);

        _height = 0;
    }

    public void AddBlock(Block block)
    {
        if (_blocksInStack.Count < _maxBlockCount && block.isAvailable)
        {
            block.transform.SetParent(transform);

            _blocksInStack.Add(block);

            block.MoveToStack(GetFreePlace());

            EventManager.TriggerEvent(GameEvent.BLOCK_TO_STACK, _blocksInStack.Count, _maxBlockCount);
        }
    }

    public void ClearStack(Transform barn)
    {
        int coins = 0;
        foreach (var block in _blocksInStack)
        {
            coins += block.Cost;
        }

        EventManager.TriggerEvent(GameEvent.ADD_MONEY, coins, _blocksInStack.Count, _maxBlockCount, timeForMovingToBarn);
        Coins += coins;

        StartCoroutine(MoveBlocksToBarn(barn));

        _height = 0;
        _slots = GetSlotsForLevel(_height);
    }

    IEnumerator MoveBlocksToBarn(Transform barn)
    {
        IsBlocksMoving = true;
        for (var i = _blocksInStack.Count - 1; i >= 0; i--)
        {
            _blocksInStack[i].MoveToBarn(barn);

            yield return new WaitForSeconds(timeForMovingToBarn);
        }

        _blocksInStack.Clear();
        IsBlocksMoving = false;
    }

    private Vector3 GetFreePlace()
    {
        if (!_slots.Any(x => x.Value == true))
        {
            _height += deltaY;
            _slots = GetSlotsForLevel(_height);
        }

        var vector = _slots.First(x => x.Value == true).Key;
        _slots[vector] = false;

        return vector;
    }

    private Dictionary<Vector3, bool> GetSlotsForLevel(float height)
    {
        return new Dictionary<Vector3, bool>()
        {
            { new Vector3(-deltaX - spaceBetweenX, height, deltaZ + spaceBetweenZ), true },
            { new Vector3(0f, height, deltaZ + spaceBetweenZ), true },
            { new Vector3(deltaX + spaceBetweenX, height, deltaZ + spaceBetweenZ), true },
            { new Vector3(-deltaX - spaceBetweenX, height, 0), true },
            { new Vector3(0f, height, 0), false },
            { new Vector3(deltaX + spaceBetweenX, height, 0), true },
            { new Vector3(-deltaX - spaceBetweenX, height, -deltaZ - spaceBetweenZ), true },
            { new Vector3(0f, height, -deltaZ - spaceBetweenZ), true },
            { new Vector3(deltaX + spaceBetweenX, height, -deltaZ - spaceBetweenZ), true },
        };
    }
}
