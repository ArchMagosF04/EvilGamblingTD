using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Camera cam;

    private TowerController currentPlacingTower;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (currentPlacingTower != null)
        {
            currentPlacingTower.transform.position = PlayerInputHandler.Instance.PointerScreenPosition;

            if (PlayerInputHandler.Instance.PointerRelease)
            {
                currentPlacingTower.PlaceTower();
                currentPlacingTower = null;
            }
        }
    }

    public void SetTowerToPlace(TowerController prefab)
    {
        if (currentPlacingTower != null)
        {
            CancelCurrentPlacing();
        }

        currentPlacingTower = TowerPool.Instance.GetTower(prefab, PlayerInputHandler.Instance.PointerScreenPosition, Quaternion.identity);
    }

    public void CancelCurrentPlacing()
    {
        TowerPool.Instance.ReturnToPool(currentPlacingTower.ID, currentPlacingTower);

        currentPlacingTower = null;
    }
}
