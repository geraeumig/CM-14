﻿using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared._CM14.Scoping;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ScopeComponent : Component
{
    /// <summary>
    /// The entity that's scoping
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? User;

    /// <summary>
    /// Is it currently scoped in
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public bool IsScoping;

    /// <summary>
    /// Value to which zoom will be set when scoped in
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Zoom = 1f;

    /// <summary>
    /// Last position of the user when scope was toggled on
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityCoordinates LastScopedAt;

    /// <summary>
    /// Helper entity, which is spawned to load far chunks
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? PvsLoader;

    [DataField]
    public EntProtoId PvsLoaderProto = "CMScopingChunkLoader";

    [DataField]
    public EntProtoId ScopingToggleAction = "CMActionToggleScope";

    [DataField, AutoNetworkedField]
    public EntityUid? ScopingToggleActionEntity;
}
