# Back to the Button

A 2D puzzle-platformer built in Unity around a single core mechanic: **press a button to rewind time and snap back to your shadow**.

## Gameplay

The player navigates platformer levels where progress requires reaching buttons while managing a time-rewind ability. Activating the ability returns the player to where their ghost shadow was a few seconds ago — crossing gaps, escaping traps, or triggering sequences that require being in two places in sequence.

## Technical Highlights

### Time-Rewind System (`EkkoUlt.cs`)
- Continuously records player position, animation frame, and facing direction into a rolling `Queue<Vector2>` over a configurable time window
- On activation, converts the queue into a reverse stack and replays it — snapping the player back through their own history frame by frame
- Slows `Time.timeScale` to 7.5% during rewind for visual effect
- Drives a post-processing `Volume` weight via `Mathf.Lerp` for a chromatic aberration/VHS effect
- Renders a ghost shadow at the delayed position so the player can see exactly where they'll land
- Cooldown system prevents spam; collision edge-case handler (ShadowBugControl) prevents players from phasing into doors on recall

### Jump System (`Jump.cs`)
- **Coyote time** — grounded memory window so jumps still register just after walking off a ledge
- **Jump buffering** — input memory window so early button presses aren't dropped
- **Variable-height cutting** — releasing jump early reduces velocity, giving the player control over arc height
- **Corner pushing** — raycasts detect partial ceiling overlap and nudge the player sideways to prevent getting stuck mid-air on edges
- Double jump with unlock flag

### Other Systems
- **Inventory** (`Inventory.cs`) — ScriptableObject-backed item list with UI instantiation and audio feedback
- **Moving platforms** with player parenting
- **Parallax scrolling** with configurable depth layers
- **Full audio manager** with effect playback by name
- **Post-processing** via URP Volume stack
- **WebGL build** — runs in browser

## Stack

- Unity 2022 (2D URP)
- C#
- Unity Tilemap + SpriteShape
- Universal Render Pipeline
- WebGL export

## What I Learned

Implementing the rewind mechanic from scratch (rather than using a third-party asset) forced a clear model of what "game state" actually means — what needs to be recorded, what can be derived, and how to replay it without physics artifacts. The time-scale trick with post-processing is the kind of detail that goes unnoticed when it works and immediately breaks immersion when it doesn't.
