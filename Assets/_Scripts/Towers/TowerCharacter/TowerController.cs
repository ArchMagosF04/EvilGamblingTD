using Alchemy.Inspector;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_TowerData TowerData {  get; private set; }
    [BoxGroup("Components"), SerializeField] private SpriteRenderer[] spriteRenderers;
    [BoxGroup("Components"), SerializeField] private BoxCollider towerCollider;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder;

    [BoxGroup("Placement Settings"), SerializeField] private LayerMask obstructLayer;

    public bool TowerPlaced { get; private set; }
    public bool CanPlace { get; private set; }
    private bool returned;

    private void Awake()
    {
        if (!towerCollider) towerCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        returned = false;
        towerCollider.isTrigger = true;
        TowerPlaced = false;

        foreach (var sprite in spriteRenderers)
        {
            Color tempColor = sprite.color;
            tempColor.a = 0.4f;
            sprite.color = tempColor;
        }
    }

    public void ReceiveTowerData(SO_TowerData data) => TowerData = data;

    #region Tower Placement

    public void PlaceTower()
    {
        TowerPlaced = true;
        towerCollider.isTrigger = false;

        spriteRenderOrder.UpdateOrderOfLayers();

        foreach (var sprite in spriteRenderers)
        {
            Color tempColor = sprite.color;
            tempColor.a = 1f;
            tempColor = Color.white;
            sprite.color = tempColor;
        }
    }

    public void TogglePlaceAvailability(bool toggle) => CanPlace = toggle;

    public bool IsTowerObstructed()
    {
        Vector3 boxCenter = transform.position + towerCollider.center;
        Vector3 halfExtends = towerCollider.size / 2;

        if (Physics.CheckBox(boxCenter, halfExtends, Quaternion.identity, obstructLayer, QueryTriggerInteraction.Ignore))
        {
            SignalTowerObstructed();

            return true;
        }
        else
        {
            SignalTowerIsPlaceable();

            return false;
        }
    }

    public void SignalTowerObstructed()
    {
        if (!CanPlace) return;
        CanPlace = false;

        foreach (var sprite in spriteRenderers)
        {
            Color tempColor = Color.red;
            tempColor.a = 0.4f;
            sprite.color = tempColor;
        }
    }

    public void SignalTowerIsPlaceable()
    {
        if (CanPlace) return;
        CanPlace = true;

        foreach (var sprite in spriteRenderers)
        {
            Color tempColor = Color.white;
            tempColor.a = 0.4f;
            sprite.color = tempColor;
        }
    }

    #endregion

    public void DestroyTower()
    {
        if (TowerPool.Instance != null)
        {
            if (!returned)
            {
                TowerPool.Instance.ReturnToPool(TowerData.ID, this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
