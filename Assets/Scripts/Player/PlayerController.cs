using System;
using GameInputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    private Rigidbody _rigidbody;
    private Camera _mainCamera;

    
    public float CameraDistance = 5;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        Instance = this;
    }

    void Update()
    {
        Instance = this;
        var cameraPosition = transform.position + new Vector3(CameraDistance,CameraDistance,-CameraDistance);
        _mainCamera.transform.position = Vector3.Slerp(_mainCamera.transform.position, cameraPosition, Time.deltaTime * 5f);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(InputSystem.Move.x, 0, InputSystem.Move.y);
        
        movement = _mainCamera.transform.TransformDirection(movement);
        movement.y = 0;
        
        movement = movement.normalized;
        
        Vector3 velocity = movement * 5f;
        velocity.y = _rigidbody.linearVelocity.y;

        _rigidbody.linearVelocity = velocity;
        if (movement != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, movement, Time.fixedDeltaTime * 10f);
        }
    }
}
