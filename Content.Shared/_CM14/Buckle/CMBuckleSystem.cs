﻿using Content.Shared.Buckle.Components;
using Robust.Shared.Physics.Events;

namespace Content.Shared._CM14.Buckle;

public sealed class CMBuckleSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;

    private readonly HashSet<EntityUid> _intersecting = new();

    public override void Initialize()
    {
        SubscribeLocalEvent<BuckleClimbableComponent, BuckleChangeEvent>(OnBuckleClimbableBuckleChange);

        SubscribeLocalEvent<ActiveBuckleClimbingComponent, PreventCollideEvent>(OnBuckleClimbablePreventCollide);
    }

    private void OnBuckleClimbableBuckleChange(Entity<BuckleClimbableComponent> ent, ref BuckleChangeEvent args)
    {
        if (args.StrapEntity != ent.Owner)
            return;

        var active = EnsureComp<ActiveBuckleClimbingComponent>(args.BuckledEntity);
        active.Strap = ent;
        Dirty(args.BuckledEntity, active);
    }

    private void OnBuckleClimbablePreventCollide(Entity<ActiveBuckleClimbingComponent> ent, ref PreventCollideEvent args)
    {
        if (args.Cancelled)
            return;

        if (ent.Comp.Strap == args.OtherEntity)
            args.Cancelled = true;
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveBuckleClimbingComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.Strap is not { } strap)
            {
                RemCompDeferred<ActiveBuckleClimbingComponent>(uid);
                continue;
            }

            _intersecting.Clear();
            _entityLookup.GetEntitiesIntersecting(uid, _intersecting);

            if (!_intersecting.Contains(strap))
                RemCompDeferred<ActiveBuckleClimbingComponent>(uid);
        }
    }
}
