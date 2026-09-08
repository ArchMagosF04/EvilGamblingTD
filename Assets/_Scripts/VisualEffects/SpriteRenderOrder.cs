using Alchemy.Inspector;
using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    [HelpBox("Place in ascending order starting with the one in the back")]
    [SerializeField] private SpriteRenderer[] spriteArray;
    [SerializeField] private bool orderOnAwake = false;

    private void Awake()
    {
        if (orderOnAwake) UpdateOrderOfLayers();
    }

    public void UpdateOrderOfLayers()
    {
        for (int i = 0; i < spriteArray.Length; i++)
        {
            spriteArray[i].sortingOrder = i + Mathf.RoundToInt(Mathf.Abs(transform.position.y) * 100);
        }
    }

    public void BringToFront()
    {
        for (int i = 0; i < spriteArray.Length; i++)
        {
            spriteArray[i].sortingOrder = short.MaxValue;
        }
    }
}
