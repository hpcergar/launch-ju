using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private Vector3 startingPosition;
    [SerializeField] private Vector3 movementVector;
    private float movementFactor;
    [SerializeField] float period = 2f;
    [SerializeField] float delay = 0f;

    const float tau = Mathf.PI * 2;

    private bool isPaused = false;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused) {return;}
        if (period <= Mathf.Epsilon) { return; } // Avoid division by zero. Use Epsilon to compare floats
        if(delay > 0f && Time.time < delay) { return; }
        float cycles = (Time.time - delay) / period; // Always growing over time
        float rawSinWave = Mathf.Sin(cycles * tau); // Between -1 and 1
        movementFactor = (rawSinWave + 1f) / 2f; // Keep it between 0 and 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }

    public void Pause()
    {
        isPaused = true;
    }
}
