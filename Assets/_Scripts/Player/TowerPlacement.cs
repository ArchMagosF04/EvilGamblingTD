using Alchemy.Inspector;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private Camera cam;

    private TowerController currentPlacingTower;
    private SO_TowerData currentTowerData;

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
                Vector3 pos = PlayerInputHandler.Instance.GetPointerWorldPosition();
                pos.z = 0;

                currentPlacingTower.transform.position = pos;
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

    public void SetTowerToPlace(SO_TowerData data)
    {
        if (currentPlacingTower != null)
        {
            CancelCurrentPlacing();
        }

        currentTowerData = data;

        Vector3 pos = PlayerInputHandler.Instance.GetPointerWorldPosition();
        pos.z = 0;

        currentPlacingTower = TowerPool.Instance.GetTower(data.TowerPrefab, pos, Quaternion.identity);
    }

    public void CancelCurrentPlacing()
    {
        currentPlacingTower.DestroyTower();

        currentTowerData = null;
        currentPlacingTower = null;
    }
}
