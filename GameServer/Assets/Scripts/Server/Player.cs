﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public int targetId;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public static SpellBook spellBook = null;
    public Coroutine spellCoroutine;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Awake()
    {
        if (spellBook == null)
            spellBook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
    }
    
    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        inputs = new bool[5];
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }

        Move(_inputDirection);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        Vector3 oldPosition = transform.position;
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);
        
        if (oldPosition != transform.position)
        {
            StopSpellCasting();
        }
        
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void CastProjectile(int spellId,Vector3 viewDirection)
    {
        if (targetId != 0 && targetId != -1)
        {
            if(spellCoroutine==null)
                spellCoroutine=StartCoroutine(SpellProgress(spellId));
        }
    }

    private IEnumerator SpellProgress(int spellId)
    {
        int oldTargetId = targetId;
        Spell spell = spellBook.GetSpell(spellId);
        float timeLeft = Time.deltaTime;
        float rate = 1.0f / spell.CastTime;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            progress += rate * Time.deltaTime;
            ServerSend.CastBarProgress(progress,id);
            yield return null;
        }
        
        NetworkManager.instance.InstantiateProjectile(transform,spellId).Initialize(id,oldTargetId,spellId);
        spellCoroutine = null;
    }

    public void StopSpellCasting()
    {
        if (spellCoroutine != null)
        {
            StopCoroutine(spellCoroutine);
            spellCoroutine = null;
        }
    }
}
