using Content.Shared._MACRO.BloodCult;
using Content.Shared.EntityConditions;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared._MACRO.BloodCult.EntityConditions.Conditions;

/// <inheritdoc cref="EntityConditionSystem{T, TCondition}"/>
[UsedImplicitly]
public sealed partial class IsBloodCultistEntityConditionSystem : EntityConditionSystem<BloodCultistComponent, IsBloodCultist>
{
    protected override void Condition(Entity<BloodCultistComponent> entity, ref EntityConditionEvent<IsBloodCultist> args)
    {
        args.Result = !args.Condition.Invert;
    }
}

/// <inheritdoc cref="EntityCondition"/>
[UsedImplicitly]
public sealed partial class IsBloodCultist : EntityConditionBase<IsBloodCultist>
{
    /// <summary>
    /// If true, invert the result (check if NOT a cultist).
    /// </summary>
    [DataField]
    public bool Invert = false;

    public override string EntityConditionGuidebookText(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-is-blood-cultist", ("invert", Invert));
    }
}
