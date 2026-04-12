using Content.Shared._MACRO.BloodCult;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Nutrition;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared._MACRO.BloodCult.EntitySystems;

/// <summary>
/// Plays the holy-water burn sound when cultists ingest holy water.
/// <see cref="IngestingEvent"/> runs once per do-after completion; drinking can repeat the do-after several times
/// per use, so we debounce audio so one "sip" does not stack the same clip.
/// Metabolism does not raise this event.
/// </summary>
public sealed class HolywaterCultistIngestionSoundSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private static readonly ProtoId<ReagentPrototype> Holywater = "Holywater";

    private static readonly SoundSpecifier BurnSound = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

    /// <summary>
    /// Minimum time between burn sounds per entity (collapses multi-chunk ingestion from one drink use).
    /// </summary>
    private static readonly TimeSpan SoundDebounce = TimeSpan.FromSeconds(0.45);

    private readonly Dictionary<EntityUid, TimeSpan> _nextBurnSoundOk = new();

    public override void Initialize()
    {
        SubscribeLocalEvent<BloodCultistComponent, IngestingEvent>(OnIngesting);
    }

    /// <summary>
    /// Clears debounce state when the cultist entity is removed. Called from <see cref="BloodCultistMetabolismSystem"/>
    /// because only one <see cref="EntityTerminatingEvent"/> subscription per component type is allowed.
    /// </summary>
    public void ClearBurnSoundDebounce(EntityUid uid)
    {
        _nextBurnSoundOk.Remove(uid);
    }

    private void OnIngesting(Entity<BloodCultistComponent> ent, ref IngestingEvent args)
    {
        if (args.Split.GetTotalPrototypeQuantity(Holywater) <= FixedPoint2.Zero)
            return;

        var uid = ent.Owner;
        var now = _timing.CurTime;
        if (_nextBurnSoundOk.TryGetValue(uid, out var earliest) && now < earliest)
            return;

        _audio.PlayPvs(BurnSound, ent, BurnSound.Params);
        _nextBurnSoundOk[uid] = now + SoundDebounce;
    }
}
