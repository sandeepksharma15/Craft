using System;

namespace Craft.MediaQuery.Models;

internal class ObserverSubscription(string elementId, Guid observerId, ResizeOptions? resizeOptions = null) : IEquatable<ObserverSubscription>
{
    public string ElementId { get; } = elementId;
    public Guid ObserverId { get; } = observerId;
    public ResizeOptions? Options { get; } = resizeOptions;

    public bool Equals(ObserverSubscription other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return ElementId.Equals(other.ElementId) && ObserverId.Equals(other.ObserverId);
    }

    public override bool Equals(object obj)
        => (obj is ObserverSubscription subscription) && Equals(subscription);

    public override int GetHashCode() => HashCode.Combine(ElementId, ObserverId);

    public override string ToString()
        => $"JS Listener Id = [{ElementId}], Observer Id = [{ObserverId}]";
}
