using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBoat : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private float thrustForce = 1000f;
    [SerializeField]private float turnTorque = 2000f;
    
    private float steering = 0f;
    private float throttle = 0f;
    private bool isOnBoard = false;
    private bool isPiloting = false;
    
    [Header("Input")] 
    [SerializeField] private InputActionProperty thumbInput;
    [SerializeField] private InputActionProperty pilotingInput;
    
    [SerializeField] private XROrigin _origin;
    [SerializeField] private GameObject moveHandler;
    
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;           // Water resistance
        rb.angularDamping = 2f;    // Water rotation resistance
        isOnBoard = false;
        
        pilotingInput.action.performed += OnEnablePiloting;
    }

    private void OnEnablePiloting(InputAction.CallbackContext obj)
    {
        if (!isOnBoard) return;
        
        isPiloting = !isPiloting;
        
        moveHandler.SetActive(!isPiloting); 
    }

    public void OnBoard(bool onBoard)
    {
        isOnBoard = onBoard;
        
        if (onBoard)
        {
            _origin.transform.parent = this.gameObject.transform;
            moveHandler.SetActive(onBoard);  
            
        }
        else
        {
            _origin.transform.parent = null;
            moveHandler.SetActive(onBoard);  
        }
        
    }
    void FixedUpdate()
    {
        if (!isOnBoard) return;
        if (!isPiloting) return;
        
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
