using Content.Shared._MACRO.BloodCult;
using Content.Shared._MACRO.BloodCult.Systems;
using Content.Shared.Damage.Systems;
using Content.Shared.EntityEffects;
using Content.Shared.EntityEffects.Effects;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;

namespace Content.Shared._MACRO.BloodCult.EntityEffects;

/// <summary>
/// Accumulates decultification from reagents/effects. Crossing100 triggers full deconversion via
/// <see cref="BloodCultMindShieldSystem.TryDeconvert"/> so it works even when <see cref="Content.Server.GameTicking.Rules.BloodCultRuleSystem"/>
/// is not ticking (no active blood cult game rule).
/// </summary>
public sealed partial class DeCultifyEntityEffectSystem : EntityEffectSystem<BloodCultistComponent, DeCultify>
{
	[Dependency] private readonly INetManager _net = default!;
	[Dependency] private readonly SharedAudioSystem _audio = default!;
	[Dependency] private readonly SharedStaminaSystem _stamina = default!;
	[Dependency] private readonly BloodCultMindShieldSystem _mindShield = default!;
	[Dependency] private readonly SharedPopupSystem _popup = default!;

	protected override void Effect(Entity<BloodCultistComponent> entity, ref EntityEffectEvent<DeCultify> args)
	{
		var bloodCultist = entity.Comp;
		var scale = args.Scale;

		var oldDeCultification = bloodCultist.DeCultification;
		var newDeCultification = oldDeCultification + (args.Effect.Amount * scale);

		// Threshold: server-only deconvert (mind/role code raises net events; crashes client if run in prediction).
		// Client keeps accumulating until replicated state removes the cultist component.
		if (oldDeCultification < 100.0f && newDeCultification >= 100.0f && _net.IsServer)
		{
			if (args.Effect.DeconversionSound != null)
				_audio.PlayPvs(args.Effect.DeconversionSound, entity, args.Effect.DeconversionSound.Params);

			_stamina.TakeStaminaDamage(entity, args.Effect.DeconversionStaminaDamage, visual: false);

			_popup.PopupEntity(Loc.GetString("cult-deconverted"), entity.Owner, entity.Owner, PopupType.LargeCaution);

			_mindShield.TryDeconvert(entity.Owner, popupLocId: null, stunDuration: TimeSpan.Zero, log: true);
			return;
		}

		bloodCultist.DeCultification = newDeCultification;
		Dirty(entity);
	}
}
