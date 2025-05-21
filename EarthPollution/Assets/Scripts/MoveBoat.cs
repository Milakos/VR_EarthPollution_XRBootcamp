using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBoat : MonoBehaviour
{
    [Header("Movement Settings")]
    public float thrustForce = 1000f;
    public float turnTorque = 200f;
    public float steering = 0f;
    public float throttle = 0f;
    
    [Header("Input")] 
    [SerializeField] private InputActionProperty thumbInput;

    private Rigidbody rb;

    private void OnEnable()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;           // Water resistance
        rb.angularDamping = 2f;    // Water rotation resistance
    }

    void FixedUpdate()
    {

        steering = thumbInput.action.ReadValue<Vector2>().x;
        throttle = thumbInput.action.ReadValue<Vector2>().y;
        
        // Apply forward/backward thrust
        Vector3 force = transform.forward * throttle * thrustForce * Time.fixedDeltaTime;
        rb.AddForce(force);

        // Apply turning torque
        Vector3 torque = Vector3.up * steering * turnTorque * Time.fixedDeltaTime;
        rb.AddTorque(torque);
    }
}
