using UnityEngine;
using static UsableTile;

public class Tile : MonoBehaviour
{
    [SerializeField] private UnityEngine.Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highLight;

    public virtual void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }

    private void OnMouseEnter()
    {
        highLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highLight.SetActive(false);
    }
}
