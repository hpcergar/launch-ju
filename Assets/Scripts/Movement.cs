using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody currentRigidbody;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        currentRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        if (right && left)
        {
            return;
        }

        if (left)
        {
            rotate(Vector3.forward);
        }
        else if (right)
        {
            rotate(Vector3.back);
        }

    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            currentRigidbody.AddRelativeForce(Vector3.up * adaptDeltaValue(mainThrust));
        }
    }

    private void rotate(Vector3 vector)
    {
        currentRigidbody.freezeRotation = true;
        transform.Rotate(vector * adaptDeltaValue(rotationThrust));
        currentRigidbody.freezeRotation = false;
    }

    private float adaptDeltaValue(float value)
    {
        return Time.deltaTime * value;
    }
}
