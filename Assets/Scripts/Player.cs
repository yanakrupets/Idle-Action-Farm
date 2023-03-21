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

    [SerializeField] private Stack _stack;
    [SerializeField] private Transform _scythe;

    public bool IsShippingToBarn { get; private set; }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _scythe.gameObject.SetActive(false);

        GravityHandling();
    }

    void Update()
    {
        // gravity to another class
        GravityHandling();

        IsShippingToBarn = _stack.IsBlocksMoving;
    }

    public void Move(Vector3 direction)
    {
        if (!IsShippingToBarn)
        {
            direction = direction * _moveSpeed;
            direction.y = _currentGravity;
            _characterController.Move(direction * Time.deltaTime);
        }
    }

    public void Rotate(Vector3 direction)
    {
        if (_characterController.isGrounded && !IsShippingToBarn)
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
            var block = other.GetComponent<Block>();
            _stack.AddBlock(block);
        }

        if (other.gameObject.tag == "Barn")
        {
            _stack.ClearStack(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GardenBed")
        {
            var gardenBed = other.GetComponent<GardenBed>();
            if (gardenBed.CultureGrown)
            {
                _scythe.gameObject.SetActive(true);
                _animator.SetBool("IsMowing", true);
            }
            else
            {
                _scythe.gameObject.SetActive(false);
                _animator.SetBool("IsMowing", false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GardenBed")
        {
            _scythe.gameObject.SetActive(false);
            _animator.SetBool("IsMowing", false);
        }
    }
}
