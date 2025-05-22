using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterStylizedShader
{
    [RequireComponent(typeof(Rigidbody))]
    public class FloatingObject : MonoBehaviour
    {
        public Transform[] floaters;
        public float underWaterDrag = 3f;
        public float underWaterAngularDrag = 1f;
        public float airWaterDrag = 0f;
        public float airWaterAngularDrag = 0.05f;

        public float floatingPower = 15f;

        public float baseWaterHeight = 0f;
        public float waterHeightVariation = 2f;
        public float waveSpeed = 1.0f;
        public float waterHeight;
    
        Rigidbody rb;
        int floatersUnderwater;
        bool underwater;
        public bool isOnSea = true;
        public bool isPile = false;
        private void Awake()
        {
            floaters[0] = transform.GetChild(0);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        
        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Water"))
                OnSeaDetection(true);
        }

        public void OnSeaDetection(bool _isOnSea)
        {
            isOnSea = _isOnSea;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isOnSea) return;
            
            waterHeight = baseWaterHeight + Mathf.Sin(Time.time * waveSpeed) * (waterHeightVariation / 2f);

            floatersUnderwater = 0;
            for (int i = 0; i < floaters.Length; i++)
            {
                float diff = floaters[i].position.y - waterHeight;

                if (diff < 0)
                {
                    rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position, ForceMode.Force);
                    floatersUnderwater++;
                    if (!underwater)
                    {
                        underwater = true;
                        SwitchState(true);
                    }
                }
            }


            if (underwater && floatersUnderwater == 0)
            {
                underwater = false;
                SwitchState(false);
            }
        }

        void SwitchState(bool isUnderwater)
        {
            if (isUnderwater)
            {
                rb.linearDamping = underWaterDrag;
                rb.angularDamping = underWaterAngularDrag;
            }
            else
            {
                rb.linearDamping = airWaterDrag;
                rb.angularDamping = airWaterAngularDrag;
            }
        }
    }
}

