using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float movementSpeed = 10f;
    private InputsManager ia;
    private Vector2 moveVector = new Vector2(0f, 0f);
    private float rotateAmount;
    private GameObject cam;

    private float maxHeight = 20f, minHeight = 3f;
    private float yMove = 0f, wantedY = 20f;

    private float scrollAmount = 0f;
    private bool sbPressed = false;
    private Vector2 pressedLocation = new Vector2(0, 0);

    private void Awake()
    {
        if (Instance != null) Destroy(this); else Instance = this;

        ia = new InputsManager();
        ia.Player.MovementHorizontal.performed += msg => moveVector.x = msg.ReadValue<float>();
        ia.Player.MovementHorizontal.canceled += _ => moveVector.x = 0f;
        ia.Player.MovementVertical.performed += msg => moveVector.y = msg.ReadValue<float>();
        ia.Player.MovementVertical.canceled += _ => moveVector.y = 0f;
        ia.Player.Rotate.performed += msg => rotateAmount = msg.ReadValue<float>();
        ia.Player.Rotate.canceled += _ => rotateAmount = 0;
        ia.Player.Levitate.performed += msg => yMove = msg.ReadValue<float>();
        ia.Player.Levitate.canceled += _ => yMove = 0f;
        ia.Player.ScrollWheel.performed += msg => { scrollAmount = msg.ReadValue<float>(); MoveOnY(scrollAmount * Time.deltaTime * -1f * 50f); };
        ia.Player.ScrollButton.performed += _ => { sbPressed = true; pressedLocation = Mouse.current.position.ReadValue(); };
        ia.Player.ScrollButton.canceled += _ => sbPressed = false;
    }

    private void Start()
    {
        cam = transform.GetChild(0).gameObject;
        yMove = 0f;
        MoveOnY(1f);
        cam.transform.LookAt(transform);
    }

    public void EnableControls()
    {
        ia.Enable();
    }

    public void DisableControls()
    {
        ia.Disable();
    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    private void MovePlayerOnXZ()
    {
        if (moveVector == Vector2.zero) return;
        if (moveVector.magnitude > 1f) moveVector = moveVector.normalized;
        float xMove = moveVector.x * movementSpeed * Time.deltaTime;
        float zMove = moveVector.y * movementSpeed * Time.deltaTime;

        transform.Translate(xMove, 0, zMove);
    }

    private void HandleMouse()
    {
        if (!sbPressed) return;

        Vector2 currentVector = Mouse.current.position.ReadValue() - pressedLocation;
        float amt = currentVector.x / Screen.width * 200f;
        RotatePlayer(amt);
    }

    private void RotatePlayer(float amt)
    {
        if (amt == 0f) return;
        transform.eulerAngles = transform.eulerAngles + new Vector3(0f, amt * Time.deltaTime, 0f);
    }
    private void MoveOnY(float amt)
    {
        if (amt == 0) return;
        wantedY = Mathf.Clamp(cam.transform.position.y + amt * 20f * Time.deltaTime, minHeight, maxHeight);
        float selX = 1f / Mathf.Sqrt(wantedY);

        Vector3 wantedPos = new Vector3(0f, wantedY, -1 * (5f * selX + 10f));
        cam.transform.localPosition = wantedPos;
        cam.transform.LookAt(transform);
    }

    private void LateUpdate()
    {
        HandleMouse();
        MovePlayerOnXZ();
        RotatePlayer(rotateAmount * 100f);
        MoveOnY(yMove);
    }
}
