using System;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canvas.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            canvas.SetActive(false);
    }

    private void Update()
    {
        // canvas.transform.LookAt(-player.transform.position, Vector3.up);
    }
}
