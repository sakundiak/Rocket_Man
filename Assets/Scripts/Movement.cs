using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float liftThrust = 100f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip rocketThrust;

    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;

    Rigidbody rbody;
    AudioSource audioSource;

       void Start()
    {
        rbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        else
        {
            StopRotation();
        }
    }





    void StartThrusting()
    {
        rbody.AddRelativeForce(Vector3.up * liftThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(rocketThrust);
        }
        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainThrustParticles.Stop();
    }   

    void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Play();
        }
    }
    void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Play();
        }
    }
    void StopRotation()
    {        
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rbody.freezeRotation = true;  //freeze rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rbody.freezeRotation = false;  //unfreeze rotation so physicas system can take over again
    }
}
