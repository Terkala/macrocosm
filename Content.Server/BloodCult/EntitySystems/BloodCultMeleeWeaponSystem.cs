using Robust.Shared.Random;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Content.Shared.Popups;
using Content.Server.Popups;
using Content.Server.Hands.Systems;
using Content.Shared.BloodCult;

namespace Content.Server.BloodCult.EntitySystems;

public sealed class BloodCultMeleeWeaponSystem : EntitySystem
{
	[Dependency] private readonly IRobustRandom _random = default!;
	[Dependency] private readonly PopupSystem _popupSystem = default!;
	[Dependency] private readonly HandsSystem _hands = default!;
	[Dependency] private readonly SharedAudioSystem _audioSystem = default!;

	public override void Initialize()
	{
		base.Initialize();
		SubscribeLocalEvent<BloodCultMeleeAllyBlockedEvent>(OnAllyBlocked);
		SubscribeLocalEvent<BloodCultMeleeChaplainBlockedEvent>(OnChaplainBlocked);
	}

	private void OnAllyBlocked(BloodCultMeleeAllyBlockedEvent ev)
	{
		_popupSystem.PopupEntity(
			Loc.GetString("cult-attack-teamhit"),
			ev.User, ev.User, PopupType.MediumCaution);
	}

	private void OnChaplainBlocked(BloodCultMeleeChaplainBlockedEvent ev)
	{
		_popupSystem.PopupEntity(
			Loc.GetString("cult-attack-repelled"),
			ev.User, ev.User, PopupType.MediumCaution);
		var coordinates = Transform(ev.User).Coordinates;
		_audioSystem.PlayPvs(new SoundPathSpecifier("/Audio/Effects/holy.ogg"), coordinates);
		var offsetRandomCoordinates = coordinates.Offset(_random.NextVector2(1f, 1.5f));
		_hands.ThrowHeldItem(ev.User, offsetRandomCoordinates);
	}
}
