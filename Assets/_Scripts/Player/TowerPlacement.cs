using Alchemy.Inspector;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Camera cam;

    private TowerController currentPlacingTower;

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
                    currentPlacingTower.PlaceTower();
                    currentPlacingTower = null;
                }
                else
                {
                    CancelCurrentPlacing();
                }
            }
        }
    }

    public void SetTowerToPlace(TowerController prefab)
    {
        if (currentPlacingTower != null)
        {
            CancelCurrentPlacing();
        }

        currentPlacingTower = TowerPool.Instance.GetTower(prefab, PlayerInputHandler.Instance.GetPointerWorldPosition(), Quaternion.identity);
    }

    public void CancelCurrentPlacing()
    {
        TowerPool.Instance.ReturnToPool(currentPlacingTower.ID, currentPlacingTower);

        currentPlacingTower = null;
    }
}
