using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [Space(5)]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = .4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _controller;

    [SerializeField] private float fallSpeedLimit = 100;

    [Space(5)]

    [SerializeField] private AudioSource audioSource;

    private bool canWalk = true;

    Vector3 velocity;
    bool isGrounded;
    bool isFallLimited = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!canWalk) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(x != 0 || z != 0)
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if(audioSource.isPlaying)
                audioSource.Stop();
        }

        // Clamp input so diagonal movement cannot be faster than moving forward.
        Vector3 move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f);

        _controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        if(isFallLimited && velocity.y < -fallSpeedLimit)
            velocity.y = -fallSpeedLimit;

        _controller.Move(velocity * Time.deltaTime);
    }

    public void SetFallSpeedLimit(bool state)
    {
        isFallLimited = state;
    }

    public void ChangeCanWalkStatus(bool status)
    {
        canWalk = status;
    }
}
