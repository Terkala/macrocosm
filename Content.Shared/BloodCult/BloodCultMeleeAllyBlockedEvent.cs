using Robust.Shared.GameObjects;

namespace Content.Shared.BloodCult;

/// <summary>
/// Raised when a cult melee weapon hit was blocked by hitting a fellow cultist (or construct).
/// Server subscribes to show the team-hit popup.
/// </summary>
public sealed class BloodCultMeleeAllyBlockedEvent : EntityEventArgs
{
	public readonly EntityUid User;
	public readonly EntityUid Weapon;

	public BloodCultMeleeAllyBlockedEvent(EntityUid user, EntityUid weapon)
	{
		User = user;
		Weapon = weapon;
	}
}
