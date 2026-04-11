using Content.Shared._MACRO.Salvage; // Macro
using Content.Shared._MACRO.Salvage.Magnet; // Macro
using Content.Shared.Destructible.Thresholds;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonLayers;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Salvage.Magnet;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.Salvage;

public abstract partial class SharedSalvageSystem
{
    private readonly List<SalvageMapPrototype> _salvageMaps = new();


// Macro start - Making it so the magnet offering is defined in yml
    /* Legacy magnet offering weights, pre YAML definitions
    private readonly Dictionary<ISalvageMagnetOffering, float> _offeringWeights = new()
    {
        { new AsteroidOffering(), 4.5f },
        { new DebrisOffering(), 3.5f },
        { new SalvageOffering(), 2.0f },
    };*/
    private readonly List<RuinMapPrototype> _ruinMaps = new();
    private readonly ProtoId<WeightedRandomPrototype> _magnetOfferingWeights = "SalvageMagnetOfferings";
// Macro end
    private readonly List<ProtoId<DungeonConfigPrototype>> _asteroidConfigs = new()
    {
        "BlobAsteroid",
        "ClusterAsteroid",
        "SpindlyAsteroid",
        "SwissCheeseAsteroid"
    };

    private readonly ProtoId<WeightedRandomPrototype> _asteroidOreWeights = "AsteroidOre";

    private readonly MinMax _asteroidOreCount = new(5, 7);

    private readonly List<ProtoId<DungeonConfigPrototype>> _debrisConfigs = new()
    {
        "ChunkDebris"
    };

    public ISalvageMagnetOffering GetSalvageOffering(int seed)
    {
        var rand = new System.Random(seed);
// Macro start, making it so the offering is defined in YML
        var offeringWeights = _proto.Index(_magnetOfferingWeights);
        var typeId = offeringWeights.Pick(rand);

        switch (typeId)
        {
            case "Asteroid":
                var configId = _asteroidConfigs[rand.Next(_asteroidConfigs.Count)];
                var configProto =_proto.Index(configId);
                var layers = new Dictionary<string, int>();

                var config = new DungeonConfig
                {
                    Layers = new(configProto.Layers),
                    MaxCount = configProto.MaxCount,
                    MaxOffset = configProto.MaxOffset,
                    MinCount = configProto.MinCount,
                    MinOffset = configProto.MinOffset,
                    ReserveTiles = configProto.ReserveTiles
                };

                var count = _asteroidOreCount.Next(rand);
                var weightedProto = _proto.Index(_asteroidOreWeights);
                for (var i = 0; i < count; i++)
                {
                    var ore = weightedProto.Pick(rand);
                    config.Layers.Add(_proto.Index<OreDunGenPrototype>(ore));

                    var layerCount = layers.GetOrNew(ore);
                    layerCount++;
                    layers[ore] = layerCount;
                }

                return new AsteroidOffering
                {
                    Id = configId,
                    DungeonConfig = config,
                    MarkerLayers = layers,
                };
            case "Debris":
                var id = rand.Pick(_debrisConfigs);
                return new DebrisOffering
                {
                    Id = id
                };
            case "Salvage":
                // Salvage map seed
                _salvageMaps.Clear();
                _salvageMaps.AddRange(_proto.EnumeratePrototypes<SalvageMapPrototype>());
                _salvageMaps.Sort((x, y) => string.Compare(x.ID, y.ID, StringComparison.Ordinal));
                var mapIndex = rand.Next(_salvageMaps.Count);
                var map = _salvageMaps[mapIndex];

                return new SalvageOffering
                {
                    SalvageMap = map,
                };
            case "Ruin":
                _ruinMaps.Clear();
                _ruinMaps.AddRange(_proto.EnumeratePrototypes<RuinMapPrototype>());
                _ruinMaps.Sort((x, y) => string.Compare(x.ID, y.ID, StringComparison.Ordinal));
                var ruinIndex = rand.Next(_ruinMaps.Count);
                var ruin = _ruinMaps[ruinIndex];

                return new RuinOffering
                {
                    RuinMap = ruin,
                    StationName = string.Empty,
                };
            default:
                throw new NotImplementedException($"Salvage magnet offering type {typeId} not implemented!");
        }
    }
}
// Macro end
