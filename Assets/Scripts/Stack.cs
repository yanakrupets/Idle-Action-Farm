using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private int _rows = 3;
    private int _columns = 3;
    private float _height = 0;

    [SerializeField] private int _maxBlockCount = 40;
    private List<Block> _blocksInStack;
    private Dictionary<Vector3, bool> _slots;

    public int Coins { get; private set; }

    void Start()
    {
        _blocksInStack = new List<Block>();
        _slots = GetSlotsForLevel(_height);
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
        // animation to barn
        // for each block
        // should be in block
        // transform.SetParent(destination.transform);

        int coins = 0;
        foreach (var block in _blocksInStack)
        {
            coins += block.Cost;
        }

        EventManager.TriggerEvent(GameEvent.ADD_MONEY, coins, _maxBlockCount);
        Coins += coins;

        _blocksInStack.Clear();
    }

    private Vector3 GetFreePlace()
    {
        if (!_slots.Any(x => x.Value == true))
        {
            _height += 0.16f;
            _slots = GetSlotsForLevel(_height);
        }

        var vector = _slots.First(x => x.Value == true).Key;
        _slots[vector] = false;

        return vector;
    }

    private Dictionary<Vector3, bool> GetSlotsForLevel(float height)
    {
        var delayX = 0.01f;
        var delayZ = 0.04f;

        var x = 0.3f;
        var z = 0.2f;

        return new Dictionary<Vector3, bool>()
        {
            { new Vector3(-x + delayX, height, z + delayZ), true },
            { new Vector3(0f, height, z + delayZ), true },
            { new Vector3(x + delayX, height, z + delayZ), true },
            { new Vector3(-x + delayX, height, 0), true },
            { new Vector3(0f, height, 0), false },
            { new Vector3(x + delayX, height, 0), true },
            { new Vector3(-x + delayX, height, -z - delayZ), true },
            { new Vector3(0f, height, -z - delayZ), true },
            { new Vector3(x + delayX, height, -z - delayZ), true },
        };
    }
}
