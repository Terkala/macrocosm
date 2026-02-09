using Robust.Shared.GameObjects;

namespace Content.Shared.BloodCult;

/// <summary>
/// Raised when a cult melee weapon hit was blocked by hitting a chaplain (CultResistant).
/// Server subscribes to show repel popup, play holy sound, and throw the weapon.
/// </summary>
public sealed class BloodCultMeleeChaplainBlockedEvent : EntityEventArgs
{
	public readonly EntityUid User;
	public readonly EntityUid Weapon;

	public BloodCultMeleeChaplainBlockedEvent(EntityUid user, EntityUid weapon)
	{
		User = user;
		Weapon = weapon;
	}
}
