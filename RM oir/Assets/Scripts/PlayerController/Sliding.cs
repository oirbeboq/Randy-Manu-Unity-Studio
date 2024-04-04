using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Camera")]
    [SerializeField] public Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float slidefov;
    [SerializeField] private float slidefovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt { get; private set; }

    public GameObject Arms;
    Animator ArmsAnimator;

    private PlayerLook playerLook;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        ArmsAnimator = Arms.GetComponent<Animator>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (pm.grounded)
        {
            if (Input.GetKey("left shift") && Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
                StartSlide();

            if (Input.GetKeyUp(slideKey) && pm.sliding)
                StopSlide();
        }
       
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
       
        tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, slidefov, slidefovTime * Time.deltaTime);
        ArmsAnimator.SetBool("isSliding", true);
       

        //if(Input.GetKey(jumpKey))
        //{
        //    ArmsAnimator.SetBool("isSliding", false);
        //}
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        tilt = 0f;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, slidefovTime * Time.deltaTime);
        ArmsAnimator.SetBool("isSliding", false);
    }
}
