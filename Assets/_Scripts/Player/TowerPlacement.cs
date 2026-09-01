using Alchemy.Inspector;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Camera cam;

    private TowerController currentPlacingTower;
    private SO_TowerBuyData currentTowerData;

    [BoxGroup("Layer Info"), SerializeField] private LayerMask generalLayer;
    [BoxGroup("Layer Info"), SerializeField] private LayerMask spawnableLayer;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (currentPlacingTower != null)
        {
            Ray ray = cam.ScreenPointToRay(PlayerInputHandler.Instance.PointerRawPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 50f, spawnableLayer))
            {
                currentPlacingTower.transform.position = PlayerInputHandler.Instance.GetPointerWorldPosition();
            }

            currentPlacingTower.IsTowerObstructed();

            if (PlayerInputHandler.Instance.PointerRelease)
            {
                if (currentPlacingTower.CanPlace)
                {
                    PlayerManager.Instance.LoseMoney(currentTowerData.TowerCost);
                    currentPlacingTower.PlaceTower();
                    currentPlacingTower = null;
                    currentTowerData = null;
                }
                else
                {
                    CancelCurrentPlacing();
                }
            }
        }
    }

    public void SetTowerToPlace(SO_TowerBuyData data)
    {
        if (currentPlacingTower != null)
        {
            CancelCurrentPlacing();
        }

        currentTowerData = data;
        currentPlacingTower = TowerPool.Instance.GetTower(data.TowerPrefab, PlayerInputHandler.Instance.GetPointerWorldPosition(), Quaternion.identity);
    }

    public void CancelCurrentPlacing()
    {
        TowerPool.Instance.ReturnToPool(currentPlacingTower.ID, currentPlacingTower);

        currentTowerData = null;
        currentPlacingTower = null;
    }
}
