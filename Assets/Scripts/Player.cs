using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private float _currentGravity = 0;
    [SerializeField] private float _gravityForce = 10f;

    private Animator _animator;
    private CharacterController _characterController;

    // stack
    [SerializeField] private TMP_Text _coinsText;
    private int _coins;

    [SerializeField] private TMP_Text _blocksText;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private int _maxBlockCount = 40;
    private List<Block> _blockStack;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _blockStack = new List<Block>();

        GravityHandling();
    }

    void Update()
    {
        // gravity to another class
        GravityHandling();
    }

    public void Move(Vector3 direction)
    {
        direction = direction * _moveSpeed;
        direction.y = _currentGravity;
        _characterController.Move(direction * Time.deltaTime);
    }

    public void Rotate(Vector3 direction)
    {
        if (_characterController.isGrounded)
        {
            if (Vector3.Angle(transform.forward, direction) > 0)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _rotateSpeed, 0);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }

    public void Run(bool isRunning)
    {
        _animator.SetBool("IsRunning", isRunning);
    }

    private void GravityHandling()
    {
        // remove this method
        if (!_characterController.isGrounded)
        {
            _currentGravity -= _gravityForce * Time.deltaTime;
        }
        else
        {
            _currentGravity = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Block")
        {
            if (_blockStack.Count < _maxBlockCount)
            {
                other.gameObject.SetActive(false);
                _blockStack.Add(other.GetComponent<Block>());
                _blocksText.text = _blockStack.Count + "/" + _maxBlockCount;
                _scrollbar.size = (float) _blockStack.Count / _maxBlockCount;
            }
        }

        if (other.gameObject.tag == "Barn")
        {
            foreach (var block in _blockStack)
            {
                _coins += block.Cost;
            }
            _coinsText.text = _coins.ToString();

            _blockStack.Clear();
            _blocksText.text = 0 + "/" + _maxBlockCount;
            _scrollbar.size = 0;
        }
    }
}
