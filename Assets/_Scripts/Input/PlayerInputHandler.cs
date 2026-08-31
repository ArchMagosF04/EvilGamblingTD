using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance;

    private PlayerInput playerInput;
    private Camera cam;


    public Vector2 PointerScreenPosition { get; private set; }
    public Vector2 PointerRawPosition { get; private set; }
    public bool PointerPress { get; private set; }
    public bool PointerRelease { get; private set; }


    [SerializeField] private LayerMask mousePositionLayer;
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

        cam = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (PointerRelease && Time.time > realeaseStartTime + releasePeriodDuration) PointerRelease = false;
    }

    public void OnPointerPositionInput(InputAction.CallbackContext context)
    {
        if (!processInputs) return;

        PointerScreenPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        PointerRawPosition = context.ReadValue<Vector2>();
    }

    public Vector3 GetPointerWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(PointerRawPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mousePositionLayer))
        {
            return hitInfo.point;
        }

        return Vector3.negativeInfinity;
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
