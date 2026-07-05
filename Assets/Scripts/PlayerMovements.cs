using UnityEngine;
using UnityEngine.InputSystem;

public enum FacingDirection
{
    Down, Up, Left, Right
}

public class PlayerMovements : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("Walk frames - down")]
    public Sprite[] frontalWalkFrames;

    [Tooltip("Walk frames - up")]
    public Sprite[] backWalkFrames;

    [Tooltip("Walk frames - side")]
    public Sprite[] sideWalkFrames;

    [Header("Animation")]
    public float framesPerSecond = 8f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private FacingDirection facing = FacingDirection.Down;
    private float frameTimer;
    private int currentFrame;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        ReadInput();
        UpdateFacing();
        AnimateSprite();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void ReadInput()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                input.x -= 1f;
            }

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                input.x += 1f;
            }

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                input.y += 1f;
            }

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                input.y -= 1f;
            }
        }

        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();

            if (stick.sqrMagnitude > 0.01f)
            {
                input = stick;
            }
        }

        moveInput = input.normalized;
    }

    void UpdateFacing()
    {
        if (moveInput.sqrMagnitude < 0.01f)
        {
            return; // keep last facing
        }

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            facing = moveInput.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            facing = moveInput.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }
    }

    void AnimateSprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        Sprite[] frames = GetFramesForFacing(facing);

        if (frames == null || frames.Length == 0)
        {
            return;
        }

        spriteRenderer.flipX = facing == FacingDirection.Left;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (!isMoving)
        {
            currentFrame = 0;
            frameTimer = 0f;
            spriteRenderer.sprite = frames[0];

            return;
        }

        frameTimer += Time.deltaTime;

        float frameDuration = 1f / Mathf.Max(framesPerSecond, 0.01f);

        if (frameTimer >= frameDuration)
        {
            frameTimer -= frameDuration;
            currentFrame = (currentFrame + 1) % frames.Length;
        }

        spriteRenderer.sprite = frames[currentFrame];
    }

    Sprite[] GetFramesForFacing(FacingDirection dir)
    {
        switch (dir)
        {
            case FacingDirection.Down:
                return frontalWalkFrames;

            case FacingDirection.Up:
                return backWalkFrames;

            case FacingDirection.Left:
            case FacingDirection.Right:
                return sideWalkFrames;
                
            default:
                return frontalWalkFrames;
        }
    }
}
