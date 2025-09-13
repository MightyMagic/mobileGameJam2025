using UnityEngine;

/// <summary>
/// This script manages a "mover" prefab (like a train) with CONTINUOUS movement.
/// It uses a two-state logic:
/// 1. AT_NODE: Sits on a tile, waiting for input.
/// 2. MOVING_TO_NODE: Travels smoothly to the next valid tile, ignoring input.
/// 
/// You must feed your joystick's Vector2 output into this script's public 'moveInput' field.
/// </summary>
public class RailMover : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The prefab to spawn on the track (e.g., your train)")]
    [SerializeField] private GameObject moverPrefab;

    [Header("Movement Settings")]
    [Tooltip("How fast the mover travels between tiles (in Unity units per second)")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("How far the joystick must be pushed (magnitude) to trigger a move from a tile")]
    [SerializeField] private float inputThreshold = 0.5f;

    [Header("Joystick Input")]
    [Tooltip("Your joystick script must write its Vector2 output to this variable.")]
    public Vector2 moveInput; // Your joystick script should update this value directly

    public FixedJoystick joystick;

    // --- State Management ---
    private GameObject moverInstance; // The actual spawned object
    private TileManager tileManager;
    private TileObject currentTile;    // The tile we are currently "at" (our last destination)
    private TileObject targetTile;     // The tile we are currently "moving towards"

    void Start()
    {
        tileManager = TileManager.Instance;
        if (tileManager == null)
        {
            Debug.LogError("RailMover can't find TileManager.Instance! Disabling.", this);
            this.enabled = false;
            return;
        }

        //InitializeMover();
    }

    public void InitializeMover()
    {
        TileObject startTile = FindStartTile();

        if (startTile != null)
        {
            currentTile = startTile;
            moverInstance = Instantiate(moverPrefab, currentTile.transform.position, Quaternion.identity);

            // We start "at a node," so targetTile is null, indicating we are ready for input.
            targetTile = null;
        }
        else
        {
            Debug.LogWarning("RailMover could not find a starting tile (Occupied tile at Y=0).", this);
            this.enabled = false;
        }
    }

    TileObject FindStartTile()
    {
        foreach (TileObject tile in tileManager.occupiedTiles)
        {
            if (tile.y == 0) return tile;
        }
        return null;
    }

    void Update()
    {
        moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Don't do anything if the mover hasn't spawned
        if (moverInstance == null) return;

        // --- STATE 1: MOVING TO A NODE ---
        // If targetTile is NOT null, it means we are busy moving along a path.
        if (targetTile != null)
        {
            // We must ignore all joystick input during this state.
            // Move the object smoothly towards the target tile.
            moverInstance.transform.position = Vector3.MoveTowards(
                moverInstance.transform.position,
                targetTile.transform.position,
                moveSpeed * Time.deltaTime);

            // Check if we have arrived (or are extremely close)
            if (Vector3.Distance(moverInstance.transform.position, targetTile.transform.position) < 0.01f)
            {
                // We've arrived. Snap to the exact position.
                moverInstance.transform.position = targetTile.transform.position;

                // This tile is now our new "current" tile
                currentTile = targetTile;

                // We are no longer moving towards a target. Clear it.
                // This puts us back into STATE 2.
                targetTile = null;
            }
        }
        // --- STATE 2: AT A NODE (Waiting for Input) ---
        // If targetTile IS null, it means we are sitting idle on 'currentTile' and are ready for input.
        else
        {
            // Only proceed if the joystick is pushed past our deadzone/threshold
            if (moveInput.magnitude < inputThreshold)
            {
                return; // Not pushing hard enough, do nothing.
            }

            // Input is strong enough. Determine dominant direction (horizontal or vertical).
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Horizontal input is stronger
                if (moveInput.x > 0) TrySetNewTarget(1, 0);  // Try to go East
                else TrySetNewTarget(-1, 0); // Try to go West
            }
            else
            {
                // Vertical input is stronger
                if (moveInput.y > 0) TrySetNewTarget(0, 1);  // Try to go North
                else TrySetNewTarget(0, -1); // Try to go South
            }
        }
    }

    /// <summary>
    /// Checks if a valid, occupied tile exists at the target coordinates.
    /// If yes, it sets it as the new 'targetTile', which engages STATE 1 in the next Update frame.
    /// </summary>
    private void TrySetNewTarget(int dx, int dy)
    {
        int targetX = currentTile.x + dx;
        int targetY = currentTile.y + dy;

        TileObject potentialTarget = tileManager.TileByCoords(targetX, targetY);

        // This is the "rail switch." We only set a target IF a tile exists AND it's occupied (part of the track).
        if (potentialTarget != null && potentialTarget.occupied)
        {
            targetTile = potentialTarget; // SUCCESS! This triggers the movement state.
        }
        // else: Invalid move (off-grid or not a rail tile). Do nothing.
        // We stay in STATE 2, waiting for a VALID input direction.
    }
}