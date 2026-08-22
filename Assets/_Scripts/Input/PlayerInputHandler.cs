using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance;

    private PlayerInput playerInput;


    public Vector2 PointerPosition { get; private set; }
    public bool PointerPress { get; private set; }
    public bool PointerRelease { get; private set; }

    [SerializeField] private float releasePeriodDuration = 0.1f;
    private float realeaseStartTime;

    private bool processInputs = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (PointerRelease && Time.time > realeaseStartTime + releasePeriodDuration) PointerRelease = false;
    }

    public void OnPointerPositionInput(InputAction.CallbackContext context)
    {
        if (!processInputs) return;

        PointerPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void OnPointerPressInput(InputAction.CallbackContext context)
    {
        if (!processInputs) return;

        if (context.started)
        {
            PointerPress = true;
            PointerRelease = false;
        }
        else if (context.canceled)
        {
            PointerPress = false;
            PointerRelease = true;
            realeaseStartTime = Time.time;
        }
    }

    public void EndPointerPressInput() => PointerPress = false;
}
