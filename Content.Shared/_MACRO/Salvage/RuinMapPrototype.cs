using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._MACRO.Salvage;

[Prototype]
public sealed partial class RuinMapPrototype : IPrototype
{
    [ViewVariables] [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    /// Relative directory path to the given map
    /// </summary>
    [DataField(required: true)] public ResPath MapPath;
}

