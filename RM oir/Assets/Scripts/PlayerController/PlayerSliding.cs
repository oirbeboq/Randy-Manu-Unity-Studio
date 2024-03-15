using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding Values")]
    public float maxSlideTIme;
    public float slideForce;
    private float SlideTImer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slidekey = KeyCode.C;
    private float horizontalInput;
    private float verticalInput;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = player.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slidekey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slidekey) && pm.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        pm.sliding = true;

        player.localScale = new Vector3(player.localScale.x, slideYScale, player.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Force);
       

    }
    private void SlidingMovement()
    {
        Vector3 inputeDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(inputeDirection.normalized * slideForce, ForceMode.Force);

        SlideTImer -= Time.deltaTime;

        if (SlideTImer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;
        player.localScale = new Vector3(player.localScale.x, startYScale, player.localScale.z);

    }
}


