using UnityEngine;

public class TouchCursor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color pressColor;
    [SerializeField] private Color releaseColor;

    private void Awake()
    {
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
            transform.position = PlayerInputHandler.Instance.PointerPosition;
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
