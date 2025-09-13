using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    [SerializeField] List<TileObject> tileObjects = new List<TileObject>();
    [SerializeField] int tilesInLine = 7;

    public List<TileObject> occupiedTiles = new List<TileObject>();

    

    [Header("Tile Costs")]
    public int tileCost = 1;



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
        AssignCoordinates(); 
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
            if(BuildManager.Instance.placementStage)
            {
                tile.SetFree();
                Debug.Log("Tile is free again!");
            }
            return;
        }

        // Second check: Is this a valid placement according to game rules?
        if (IsValidPlacement(tile.x, tile.y) && TileByCoords(tile.x, tile.y).canPlace)
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

    public void RemoveTile(TileObject tile)
    {
        if (tile.occupied)
        {
            for (int i = 0; i < occupiedTiles.Count; i++)
            {
                if (tile.x == occupiedTiles[i].x && tile.y == occupiedTiles[i].y)
                {
                    occupiedTiles.RemoveAt(i);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Doesnt exist in occupied tiles array!");
        }
        
    }

    // 3. NEW: This function holds the actual game logic.
    private bool IsValidPlacement(int x, int y)
    {
        // RULE 1: If NO tiles are occupied yet, this is the first move. Any tile is valid.
        if (occupiedTiles.Count == 0)
        {
            if(y == 0)
            {
                return true;
            }
            else
            {
                Debug.Log("Can only place the first rail on the first row!");
                return false;
            }
                
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

    public void AssignCoordinates()
    {
        int x = 0;
        int y = 0;

        for(int i = 0; i < tileObjects.Count;i++)
        {
            if(x < tilesInLine)
            {
                
            }
            else
            {
                x -= tilesInLine;
                y++;
            }

            tileObjects[i].x = x;
            tileObjects[i].y = y;

            x++;
        }
    }

    public bool IsOccupiedNetConnected()
    {
        // --- 1. Handle Edge Cases ---
        // If there are no tiles or only one tile occupied, the net is "connected" by default.
        if (occupiedTiles.Count <= 1)
        {
            return true;
        }

        // --- 2. Prepare for Traversal (BFS) ---
        // We need a Set to track all tiles we've already visited. A HashSet is very fast.
        HashSet<TileObject> visitedTiles = new HashSet<TileObject>();

        // We need a Queue to manage which tiles to check next.
        Queue<TileObject> tileQueue = new Queue<TileObject>();

        // --- 3. Start the Search ---
        // Pick any occupied tile to be our starting point (the first one in the list is fine).
        TileObject startTile = occupiedTiles[0];
        tileQueue.Enqueue(startTile);
        visitedTiles.Add(startTile);

        // --- 4. Run the Traversal Loop (Flood Fill) ---
        // This loop continues as long as there are tiles in our "to-do" queue.
        while (tileQueue.Count > 0)
        {
            // Get the next tile from the queue
            TileObject currentTile = tileQueue.Dequeue();

            // Now, we must find all neighbors OF THIS TILE...
            // ...that are ALSO in the occupiedTiles list.
            foreach (TileObject otherOccupiedTile in occupiedTiles)
            {
                // Skip checking the tile against itself
                if (otherOccupiedTile == currentTile) continue;

                // Check for 4-direction adjacency (same logic from IsValidPlacement)
                int xDist = Mathf.Abs(currentTile.x - otherOccupiedTile.x);
                int yDist = Mathf.Abs(currentTile.y - otherOccupiedTile.y);

                if (xDist + yDist == 1) // This is an adjacent, occupied neighbor
                {
                    // If this neighbor is one we haven't visited yet...
                    if (!visitedTiles.Contains(otherOccupiedTile))
                    {
                        // ...add it to the set (so we don't check it again)
                        visitedTiles.Add(otherOccupiedTile);
                        // ...and add it to the queue (so we will check ITS neighbors next).
                        tileQueue.Enqueue(otherOccupiedTile);
                    }
                }
            }
        }

        // --- 5. Check the Result ---
        // After the loop finishes, 'visitedTiles' contains every tile we could reach from the start tile.
        // If the number of visited tiles equals the TOTAL number of occupied tiles,
        // it means we reached everyone, and the net is connected.
        return visitedTiles.Count == occupiedTiles.Count;
    }
}
