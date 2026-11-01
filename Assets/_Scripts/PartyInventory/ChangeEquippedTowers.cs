using Alchemy.Inspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquippedTowers : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;

    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot1;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot2;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot3;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot4;
    [BoxGroup("Party Slots"), SerializeField] private PartySlot partySlot5;

    private void OnEnable()
    {
        CheckSlotInteraction();
    }

    private void CheckSlotInteraction()
    {
        if (savedData.Slot1Tower == null) partySlot2.SlotButton.interactable = false;
        else partySlot2.SlotButton.interactable = true;

        if (savedData.Slot2Tower == null) partySlot3.SlotButton.interactable = false;
        else partySlot3.SlotButton.interactable = true;

        if (savedData.Slot3Tower == null) partySlot4.SlotButton.interactable = false;
        else partySlot4.SlotButton.interactable = true;

        if (savedData.Slot4Tower == null) partySlot5.SlotButton.interactable = false;
        else partySlot5.SlotButton.interactable = true;

        SetUpButton(savedData.Slot1Tower, ref partySlot1);
        SetUpButton(savedData.Slot2Tower, ref partySlot2);
        SetUpButton(savedData.Slot3Tower, ref partySlot3);
        SetUpButton(savedData.Slot4Tower, ref partySlot4);
        SetUpButton(savedData.Slot5Tower, ref partySlot5);
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
    }
}

[Serializable]
public struct PartySlot
{
    public Button SlotButton;
    public TMP_Text SlotText;
    public Image SlotImage;
}
