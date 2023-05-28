using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Area
{
    start, hallway, enemyArea, itemArea
}
public class Movement : MonoBehaviour
{

    EventInstance footsteps;
    bool playSound =true;
    bool playDoorSound = true;

    public Area area = Area.start;
    public bool hasObject;
    //public GameObject skeleton;
    public static bool moving = false;
    Animator anim;
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    void Start()
    {
        //footsteps
        footsteps = RuntimeManager.CreateInstance("event:/Sound FX Relaxed/Walking Sound effect");
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform);

        hasObject = false;
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        //Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        if (targetDir == Vector2.zero)
        {
            // No movement
            moving= false;
            footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            playSound = true;
        }
        else
        {
            moving = true;
            TriggerWalkingSound();

        }

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Speed + Vector3.up * velocityY;
        //Debug.Log(velocity);
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasObject && other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(2);
        }
        else if (other.CompareTag("hallwayDoor"))
        {
            Debug.Log("ok");
            this.area = Area.hallway;
        }
        else if (other.CompareTag("enemyAreaDoor"))
        {
            this.area = Area.enemyArea;
            TriggerDoorSound();
        }
        else if (other.CompareTag("door"))
        {
            TriggerDoorSound();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hallwayDoor") || other.CompareTag("enemyAreaDoor") || other.CompareTag("door"))
        {
            playDoorSound = true;

        }
        
    }
    public void TriggerWalkingSound()
    {
        if (playSound)
        {
            footsteps.start();
        }
        playSound = false;
    }
    public void TriggerDoorSound()
    {
        if (playDoorSound)
        {
            RuntimeManager.PlayOneShot("event:/Ambient/Room passing", transform.position);
        }
        playDoorSound = false;
    }
}