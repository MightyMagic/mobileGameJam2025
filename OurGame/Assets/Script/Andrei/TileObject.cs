using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public int x;
    public int y;

    public bool occupied = false; // Corrected spelling
    public bool canPlace = true;

    [SerializeField] SpriteRenderer spriteRenderer;

    Sprite originalSprite;
    public Sprite occupiedSprite;

    void Start()
    {
        //touchCollider = this.transform.GetChild(0).GetComponent<Collider2D>();
        //spriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();

        //originalSprite = spriteRenderer.sprite;
    }

    public void AssignSprites(GameObject original, Sprite occupiedSprite)
    {
        spriteRenderer = original.GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        this.occupiedSprite = occupiedSprite;    
    }

    

    void OnMouseDown()
    {
        Debug.Log("The object was clicked!");
        Clicked();
    }

    public void Clicked()
    {

        TileManager.Instance.AttemptOccupyTile(this);

        //if (!occupied)
        //{
        //    occupied = true;
        //    spriteRenderer.color = Color.red;
        //
        //}
        //else
        //{
        //
        //}

        //spriteRenderer.sprite = ocuppiedSprite;
    }

    public void SetOccupied()
    {
        occupied = true;
        //spriteRenderer.color = Color.red;

        // Uncomment this if you prefer using the sprite swap
        spriteRenderer.sprite = occupiedSprite;
    }

    public void SetFree()
    {
        BuildManager.Instance.AddResources(TileManager.Instance.tileCost);
        TileManager.Instance.RemoveTile(this);
        occupied = false;
        //spriteRenderer.color = Color.white;

        spriteRenderer.sprite = originalSprite;
    }
}
