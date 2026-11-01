using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSplashArt : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;
    [BoxGroup("Components"), SerializeField] private Image image;

    private void Awake()
    {
        if (!image) image = GetComponent<Image>();
        SelectSplashArt();
    }

    [Button]
    public void SelectSplashArt()
    {
        Sprite chosen = null;

        if (savedData.Slot1Tower != null)
        {
            chosen = savedData.Slot1Tower.InventoryImage;
            image.color = Color.white;
        }
        else if (savedData.Slot2Tower != null)
        {
            chosen = savedData.Slot2Tower.InventoryImage;
            image.color = Color.white;
        }
        else if (savedData.Slot3Tower != null)
        {
            chosen = savedData.Slot3Tower.InventoryImage;
            image.color = Color.white;
        }
        else if (savedData.Slot4Tower != null)
        {
            chosen = savedData.Slot4Tower.InventoryImage;
            image.color = Color.white;
        }
        else if (savedData.Slot5Tower != null)
        {
            chosen = savedData.Slot5Tower.InventoryImage;
            image.color = Color.white;
        }
        else
        {
            Color temp = Color.white;
            temp.a = 0;
            image.color = temp;
        }

        image.sprite = chosen;
    }
}
