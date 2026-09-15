using Alchemy.Inspector;
using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class TowerController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_TowerData TowerData {  get; private set; }
    [BoxGroup("Components"), SerializeField] private BoxCollider towerCollider;
    [BoxGroup("Components"), SerializeField] private Animator animator;
    [BoxGroup("Components"), SerializeField] private HealthController healthController;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder;
    [BoxGroup("Components"), SerializeField] private SpriteRenderer[] spriteRenderers;

    [BoxGroup("Placement Settings"), SerializeField] private LayerMask obstructLayer;

    public bool TowerPlaced { get; private set; }
    public bool CanPlace { get; private set; }
    private bool returned;

    public static readonly int idleAnim = Animator.StringToHash("Deployed");

    private void Awake()
    {
        if (!healthController) healthController = GetComponent<HealthController>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!towerCollider) towerCollider = GetComponent<BoxCollider>();
        if (!spriteRenderOrder) spriteRenderOrder = GetComponentInChildren<SpriteRenderOrder>();

        healthController.OnHealthDepleted += RemoveTower;

        //if (AttackPool.Instance != null) AttackPool.Instance.PreWarmPool(TowerData.AttackPrefab, 20);
    }

    private void OnEnable()
    {
        returned = false;
        towerCollider.isTrigger = true;
        TowerPlaced = false;
            
        spriteRenderOrder.BringToFront();

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
        healthController.InitializeHealth(TowerData.MaxHealth);

        animator.SetBool(idleAnim, true);

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

    public void RemoveTower()
    {
        animator.SetBool(idleAnim, false);

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

    [Button, BoxGroup("Components")]
    public void GetTowerSprites()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
}
