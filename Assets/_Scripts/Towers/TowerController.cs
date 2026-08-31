using UnityEngine;

public class TowerController : MonoBehaviour
{
    [field: SerializeField] public int ID {  get; private set; }

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Placement Settings")]

    public bool TowerPlaced { get; private set; }

    private void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        TowerPlaced = false;
        Color tempColor = spriteRenderer.color;
        tempColor.a = 0.4f;
        spriteRenderer.color = tempColor;
    }

    public void PlaceTower()
    {
        TowerPlaced = true;

        Color tempColor = spriteRenderer.color;
        tempColor.a = 1f;
        spriteRenderer.color = tempColor;
    }
}
