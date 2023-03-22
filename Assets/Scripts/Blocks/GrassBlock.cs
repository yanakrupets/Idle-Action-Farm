using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : Block
{
    [SerializeField] private int _cost;

    public override int Cost { get; set; }

    private void Start()
    {
        Cost = _cost;
    }
}
