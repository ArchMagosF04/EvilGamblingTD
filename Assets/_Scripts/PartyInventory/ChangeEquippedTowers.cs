using Alchemy.Inspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquippedTowers : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;
    [BoxGroup("Components"), SerializeField] private Sprite emptySprite;

    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot1;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot2;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot3;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot4;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot5;

    private int selectedSlot;

    private void OnEnable()
    {
        selectedSlot = 1;
        CheckSlotInteraction();
        AssignSlotSelection();
    }

    private void CheckSlotInteraction()
    {
        SetUpButton(savedData.Slot1Tower, ref partySlot1);
        SetUpButton(savedData.Slot2Tower, ref partySlot2);
        SetUpButton(savedData.Slot3Tower, ref partySlot3);
        SetUpButton(savedData.Slot4Tower, ref partySlot4);
        SetUpButton(savedData.Slot5Tower, ref partySlot5);
    }

    private void AssignSlotSelection()
    {
        partySlot1.SlotButton.onClick.AddListener(() => selectedSlot = 1);
        partySlot2.SlotButton.onClick.AddListener(() => selectedSlot = 2);
        partySlot3.SlotButton.onClick.AddListener(() => selectedSlot = 3);
        partySlot4.SlotButton.onClick.AddListener(() => selectedSlot = 4);
        partySlot5.SlotButton.onClick.AddListener(() => selectedSlot = 5);
    }

    private void SetUpButton(SO_TowerData data, ref PartySlot partySlot)
    {
        if (data != null)
        {
            partySlot.SlotButton.interactable = true;
            partySlot.SlotText.text = data.TowerName;
            partySlot.SlotImage.color = Color.white;
            partySlot.SlotImage.sprite = data.InventoryImage;
        }
        else
        {
            partySlot.SlotText.text = "Empty";
            partySlot.SlotImage.color = Color.black;
            partySlot.SlotImage.sprite = emptySprite;
        }
    }

    public void SetSelectedTower(SO_TowerData selectedTower)
    {
        if (selectedTower != null)
        {
            if (selectedTower == savedData.Slot1Tower) return;
            if (selectedTower == savedData.Slot2Tower) return;
            if (selectedTower == savedData.Slot3Tower) return;
            if (selectedTower == savedData.Slot4Tower) return;
            if (selectedTower == savedData.Slot5Tower) return;
        }

        switch (selectedSlot)
        {
            case 1:

                SetUpButton(selectedTower, ref partySlot1);
                savedData.Slot1Tower = selectedTower;

                break;
            case 2:

                SetUpButton(selectedTower, ref partySlot2);
                savedData.Slot2Tower = selectedTower;

                break;
            case 3:

                SetUpButton(selectedTower, ref partySlot3);
                savedData.Slot3Tower = selectedTower;

                break;
            case 4:

                SetUpButton(selectedTower, ref partySlot4);
                savedData.Slot4Tower = selectedTower;

                break;
            case 5:

                SetUpButton(selectedTower, ref partySlot5);
                savedData.Slot5Tower = selectedTower;

                break;
            default:

                SetUpButton(selectedTower, ref partySlot1);
                savedData.Slot1Tower = selectedTower;

                break;
        }
    }
}

[Serializable]
public struct PartySlot
{
    public Button SlotButton;
    public TMP_Text SlotText;
    public Image SlotImage;
}
