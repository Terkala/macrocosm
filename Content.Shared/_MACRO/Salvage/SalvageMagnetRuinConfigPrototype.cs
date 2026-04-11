using System.Collections.Immutable;
using System.Collections.Generic;
using Robust.Shared.Prototypes;

namespace Content.Shared._MACRO.Salvage;

/// <summary>
/// Configuration for salvage magnet ruin generation, including damage simulation parameters.
/// Defaults are used as fallback when fields are omitted from salvage_magnet_ruin_config.yml.
/// </summary>
[Prototype]
public sealed partial class SalvageMagnetRuinConfigPrototype : IPrototype
{
    [ViewVariables] [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    /// Number of cost points to use for flood-fill algorithm.
    /// Higher values result in larger ruins. If you increase this, also increase RuinSpawnDistance.
    /// </summary>
    [DataField]
    public int FloodFillPoints = 25;

    /// <summary>
    /// Distance (in tiles) at which ruins spawn from the salvage magnet.
    /// Ruins spawn this distance away in the direction the magnet is facing.
    /// </summary>
    [DataField]
    public float RuinSpawnDistance = 82f;

    /// <summary>
    /// Chance (0.0 to 1.0) that a wall entity will be destroyed and not spawned.
    /// </summary>
    [DataField]
    public float WallDestroyChance = 0.30f;

    /// <summary>
    /// Chance (0.0 to 1.0) that a window entity will be spawned in a damaged state.
    /// </summary>
    [DataField]
    public float WindowDamageChance = 0.10f;

    /// <summary>
    /// Chance (0.0 to 1.0) that a floor tile will be replaced with lattice.
    /// Lattice tiles are never damaged further (they're already the most damaged state).
    /// </summary>
    [DataField]
    public float FloorToLatticeChance = 0.15f;

    /// <summary>
    /// Path cost for walls. Higher values make flood-fill avoid walls more.
    /// </summary>
    [DataField]
    public int WallCost = 20;

    /// <summary>
    /// Path cost for window entities by pattern. Keys are substrings matched case-insensitively against prototype IDs.
    /// Longest matching pattern wins. E.g. "windoorsecure" matches before "windoor".
    /// </summary>
    [DataField]
    public Dictionary<string, int> WindowCosts = new()
    {
        ["firelock"] = 10,
        ["windoorsecure"] = 10,
        ["windoor"] = 4,
        ["reinforceddirectional"] = 10,
        ["directional"] = 10,
        ["reinforceddiagonal"] = 10,
        ["diagonal"] = 10,
        ["reinforced"] = 10,
    };

    /// <summary>
    /// Default path cost for windows when no pattern in WindowCosts matches.
    /// </summary>
    [DataField]
    public int DefaultWindowCost = 4;

    /// <summary>
    /// Path cost for tile definitions by pattern. Keys are substrings matched case-insensitively against tile IDs.
    /// Longest matching pattern wins. Wall tiles use WallCost (via WallTileIds), not this.
    /// </summary>
    [DataField]
    public Dictionary<string, int> TileCosts = new()
    {
        ["directionalglass"] = 8,
        ["reinforcedglass"] = 12,
        ["glass"] = 8,
        ["grille"] = 4,
    };

    /// <summary>
    /// Default path cost for tiles when no pattern in TileCosts matches (floors, plating, etc.).
    /// </summary>
    [DataField]
    public int DefaultTileCost = 1;

    /// <summary>
    /// Path cost for space tiles (tiles not in the map).
    /// Set to a very high value (9999) to make flood-fill treat them as impassable.
    /// Lower values would allow flood-fill to cross small gaps of space.
    /// </summary>
    [DataField]
    public int SpaceCost = 9999;

    /// <summary>
    /// Number of flood-fill stages to perform. Each stage starts from the previous stage's frontier
    /// (tiles that were almost added but exceeded budget), creating irregular branching shapes.
    /// </summary>
    [DataField]
    public int FloodFillStages = 7;

    /// <summary>
    /// Tile definition IDs that are considered wall tiles for cost map building.
    /// Used by GetTileCost to apply WallCost to wall tiles in the tilemap.
    /// </summary>
    [DataField]
    public ImmutableList<string> WallTileIds = ImmutableList.CreateRange(new[]
    {
        "WallSolid",
        "WallReinforced",
        "WallReinforcedRust",
        "WallSolidRust",
        "WallSolidDiagonal",
        "WallReinforcedDiagonal"
    });

    /// <summary>
    /// Entity prototype IDs that are considered walls when parsing ruin maps.
    /// Used to identify wall entities in the map file for cost map building.
    /// </summary>
    [DataField]
    public ImmutableList<EntProtoId> WallPrototypes = ImmutableList.CreateRange(new EntProtoId[]
    {
        "WallSolid",
        "WallReinforced",
        "WallReinforcedRust",
        "WallSolidRust",
        "WallSolidDiagonal",
        "WallReinforcedDiagonal",
        "WallShuttleDiagonal",
        "WallPlastitaniumDiagonal",
        "WallMiningDiagonal"
    });

    /// <summary>
    /// Entity prototype IDs that are considered windows when parsing ruin maps.
    /// Includes windows, windoors, firelocks, frames, and assemblies.
    /// Used to identify window entities in the map file for cost map building.
    /// </summary>
    [DataField]
    public ImmutableList<EntProtoId> WindowPrototypes = ImmutableList.CreateRange(new EntProtoId[]
    {
        "Window",
        "WindowDirectional",
        "ReinforcedWindow",
        "WindowReinforcedDirectional",
        "WindowDiagonal",
        "ReinforcedWindowDiagonal",
        "TintedWindow",
        "WindowFrostedDirectional",
        "ShuttleWindow",
        "ShuttleWindowDiagonal",
        "PlastitaniumWindow",
        "PlastitaniumWindowDiagonal",
        "Windoor",
        "WindoorSecure",
        "WindoorPlasma",
        "WindoorSecurePlasma",
        "WindoorClockwork",
        "Firelock",
        "FirelockGlass",
        "FirelockEdge",
        "FirelockFrame",
        "WindoorAssembly",
        "WindoorAssemblySecure",
        "WindoorAssemblyPlasma",
        "WindoorAssemblySecurePlasma"
    });
}

