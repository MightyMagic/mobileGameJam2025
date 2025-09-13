using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    [SerializeField] List<TileObject> tileObjects = new List<TileObject>();
    private List<TileObject> occupiedTiles = new List<TileObject>();

    [Header("Tile Costs")]
    [SerializeField] private int tileCost = 1;



    // 2. Awake runs before Start() and is used to set up the singleton
    private void Awake()
    {
        // Check if an Instance already exists
        if (Instance != null && Instance != this)
        {
            // If yes, and it's not this one, destroy this new (duplicate) object
            Debug.LogWarning("Duplicate TileManager found. Destroying new instance.");
            Destroy(gameObject);
        }
        else
        {
            // If no instance exists, this is the first one. Set it.
            Instance = this;

            // Optional: If this manager needs to survive loading new scenes, uncomment this line:
            // DontDestroyOnLoad(gameObject);
        }
    }
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 2. NEW: This method is called BY the tile when it is clicked.
    public void AttemptOccupyTile(TileObject tile)
    {
        // First check: Is this specific tile already occupied? If yes, do nothing.
        if (tile.occupied)
        {
            Debug.Log("Tile already occupied.");
            return;
        }

        // Second check: Is this a valid placement according to game rules?
        if (IsValidPlacement(tile.x, tile.y))
        {

            // Check resource rules by asking the BuildManager
            // This line is the only change in this function.
            bool purchaseSuccessful = BuildManager.Instance.AttemptToSpend(tileCost);

            // If BOTH location AND resources are valid:
            if (purchaseSuccessful)
            {
                tile.SetOccupied();
                occupiedTiles.Add(tile);
            }
        }
        else
        {
            // If invalid (not adjacent): Do nothing (or play an error sound)
            Debug.Log("Invalid move. Must place adjacent to an existing tile.");
        }
    }

    // 3. NEW: This function holds the actual game logic.
    private bool IsValidPlacement(int x, int y)
    {
        // RULE 1: If NO tiles are occupied yet, this is the first move. Any tile is valid.
        if (occupiedTiles.Count == 0)
        {
            return true;
        }

        // RULE 2: If tiles DO exist, check if this tile is adjacent to ANY of them.
        foreach (TileObject existingTile in occupiedTiles)
        {
            // We need to check for neighbors (North, South, East, West).
            // We calculate the distance on the X-axis and Y-axis.
            int xDist = Mathf.Abs(existingTile.x - x);
            int yDist = Mathf.Abs(existingTile.y - y);

            // This math trick checks for 4-direction adjacency:
            // If (distX is 1 AND distY is 0) OR (distX is 0 AND distY is 1),
            // the sum will equal exactly 1.
            // Diagonals (1,1) would equal 2. Same tile (0,0) would equal 0.
            if (xDist + yDist == 1)
            {
                return true; // Found at least one valid neighbor. This move is allowed.
            }
        }

        // If the loop finishes without finding any adjacent neighbors, the move is invalid.
        return false;
    }

    public TileObject TileByCoords(int x, int y)
    {
        for (int i = 0; i < tileObjects.Count; i++)
        {
            if (tileObjects[i].x == x && tileObjects[i].y == y)
            {
                return tileObjects[i];
            }
        }

        return null;
    }
}
