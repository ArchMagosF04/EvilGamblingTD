using UnityEngine;

public class TouchCursor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color pressColor;
    [SerializeField] private Color releaseColor;
    [SerializeField] private LayerMask mask;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.color = normalColor;
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.PointerPress)
        {
            Ray ray = cam.ScreenPointToRay(PlayerInputHandler.Instance.PointerRawPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mask))
            {
                transform.position = hitInfo.point;
            }

            spriteRenderer.color = pressColor;
        }
        else if (PlayerInputHandler.Instance.PointerRelease)
        {
            spriteRenderer.color = releaseColor;
        }
        else
        {
            spriteRenderer.color = normalColor;
        }
    }
}
