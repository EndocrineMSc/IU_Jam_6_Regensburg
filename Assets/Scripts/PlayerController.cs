using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Field and Properties

    private Rigidbody _rigidbody;

    [SerializeField] private float _speed = 5f;
    private Vector3 _playerInput = new();


    #endregion

    #region Methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        _playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    private void MovePlayer()
    {
        _rigidbody.AddForce(_playerInput * _speed);
    }

    #endregion
}
