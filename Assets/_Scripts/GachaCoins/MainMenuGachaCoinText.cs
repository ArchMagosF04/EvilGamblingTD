using Alchemy.Inspector;
using TMPro;
using UnityEngine;

public class MainMenuGachaCoinText : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private TMP_Text textField;
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;

    private void Awake()
    {
        if (!textField) textField = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateTextValue();
    }

    public void UpdateTextValue()
    {
        textField.text = savedData.gachaCoins.ToString();
    }
}
