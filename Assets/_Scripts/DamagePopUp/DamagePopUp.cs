using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopUp : MonoBehaviour
{
    public float damage;
    public float life =0.2f;
    public TMP_Text damageText;
    
    private void Start()
    {
        damageText = GetComponent<TMP_Text>();
        damageText.text = damage.ToString();
        Destroy(gameObject, 2);

        transform.DOMove(new Vector2(transform.position.x, transform.position.y + 0.7f), 0.5f, false);
    } 
}
