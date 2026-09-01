using Alchemy.Inspector;
using UnityEngine;

public class TowerButtonsInitializer : MonoBehaviour
{
    [SerializeField] private SO_SelectedTowersArray selectedTowersArray;

    [BoxGroup("Tower Buttons"), SerializeField] private BuyTowerButton button1;
    [BoxGroup("Tower Buttons"), SerializeField] private BuyTowerButton button2;
    [BoxGroup("Tower Buttons"), SerializeField] private BuyTowerButton button3;
    [BoxGroup("Tower Buttons"), SerializeField] private BuyTowerButton button4;
    [BoxGroup("Tower Buttons"), SerializeField] private BuyTowerButton button5;

    private void Awake()
    {
        InitializeTowerButtons();
    }

    public void InitializeTowerButtons()
    {
        button1.InitializeTowerSlot(selectedTowersArray.Slot1Tower);
        button2.InitializeTowerSlot(selectedTowersArray.Slot2Tower);
        button3.InitializeTowerSlot(selectedTowersArray.Slot3Tower);
        button4.InitializeTowerSlot(selectedTowersArray.Slot4Tower);
        button5.InitializeTowerSlot(selectedTowersArray.Slot5Tower);
    }
}
