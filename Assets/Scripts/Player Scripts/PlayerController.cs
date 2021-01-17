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

    public float maxDistance = 20f, minDistance = 5f;
    private float yMove = 0f;

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
        ia.Player.ScrollWheel.performed += msg => { scrollAmount = msg.ReadValue<float>(); MoveOnY(scrollAmount); };
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
        float xMove = moveVector.x * movementSpeed * Time.unscaledDeltaTime;
        float zMove = moveVector.y * movementSpeed * Time.unscaledDeltaTime;

        transform.Translate(xMove, 0, zMove);
    }

    private void HandleMouse()
    {
        if (!sbPressed) return;

        Vector2 cur = Mouse.current.position.ReadValue();
        Vector2 currentVector = cur - pressedLocation;
        RotatePlayer(currentVector);
        pressedLocation = cur;
    }

    private void RotatePlayer(Vector2 amt)
    {
        if (amt == Vector2.zero) return;
        float xAmt = (amt.x / Screen.width) * 360f;
        float yAmt = (amt.y / Screen.height) * -360f;
        float deg = Mathf.Rad2Deg * Vector3.Angle(new Vector3(0f, 0f, -1f), cam.transform.localPosition.normalized) / 90f;
        yAmt = -Mathf.Clamp(-yAmt, deg - 50f, deg - 5f);
        transform.eulerAngles = transform.eulerAngles + new Vector3(0f, xAmt, 0f);
        cam.transform.RotateAround(transform.position, transform.right, yAmt);
        cam.transform.LookAt(transform);
    }

    private void MoveOnY(float amt)
    {
        float mv = amt * Time.unscaledDeltaTime * 6f;
        Vector3 newPos = Vector3.MoveTowards(cam.transform.localPosition, Vector3.zero, mv);
        float newDis = Vector3.Distance(newPos, Vector3.zero);
        if (newDis > maxDistance) newPos = cam.transform.localPosition.normalized * maxDistance;
        else if (newDis < minDistance) newPos = cam.transform.localPosition.normalized * minDistance;
        else if (newPos.y <= 0f) { newPos.y *= -1; newPos = cam.transform.localPosition.normalized * minDistance; }
        cam.transform.localPosition = newPos;
    }

    private void Update()
    {
        HandleMouse();
        MovePlayerOnXZ();
        MoveOnY(yMove);
        RotatePlayer(new Vector2(rotateAmount, 0f));
    }
}
