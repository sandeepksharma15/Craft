using System.Collections;
using Microsoft.Extensions.Logging;

namespace Craft.Utilities.Managers;

public class ObserverManager<TId, T>(ILogger<ObserverManager<TId, T>> logger) : IEnumerable<IObserver<T>>
{
    public int Count => Observers.Count;
    public void Clear() => Observers.Clear();

    public Dictionary<TId, IObserver<T>> Observers { get; } = [];
    public IEnumerator<IObserver<T>> GetEnumerator()
        => Observers.Select(observer => observer.Value).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Subscribe(TId id, IObserver<T> observer)
    {
        bool isNewObserver = !Observers.TryGetValue(id, out var entry);
        Observers[id] = observer;

        if (logger.IsEnabled(LogLevel.Debug))
        {
            string action = isNewObserver ? "Adding" : "Updating";
            logger.LogDebug($"{action} entry for {id}/{observer}. {Count} total observers after {action}.", action, id, observer, Observers.Count);
        }
    }

    public void Unsubscribe(TId id)
    {
        Observers.Remove(id, out _);

        logger.LogDebug("Observer unsubscribed");
    }

    public void Notify(T value, Func<TId, IObserver<T>, bool>? predicate = null)
    {
        var defunctObservers = default(List<TId>); 

        foreach (var observer in Observers)
        {
            // Skip observers which don't match the provided predicate.
            if (predicate != null && !predicate(observer.Key, observer.Value))
                continue;

            try
            {
                observer.Value.OnNext(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error notifying observer");
                defunctObservers ??= [];
                defunctObservers.Add(observer.Key);
            }
        }

        // Remove any observers that errored out
        defunctObservers?.ForEach(id => Unsubscribe(id));
    }
}
