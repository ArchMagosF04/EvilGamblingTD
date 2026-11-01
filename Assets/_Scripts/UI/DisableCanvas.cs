using Alchemy.Inspector;
using UnityEngine;

public class DisableCanvas : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (!canvas) canvas = GetComponent<Canvas>();

        canvas.enabled = false;
    }
}
