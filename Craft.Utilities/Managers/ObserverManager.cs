using System.Collections;
using Microsoft.Extensions.Logging;

namespace Craft.Utilities.Managers;

public class ObserverManager<T>(ILogger<ObserverManager<T>> logger) : IEnumerable<IObserver<T>>
{
    public int Count => Observers.Count;
    public void Clear() => Observers.Clear();
    public List<IObserver<T>> Observers { get; } = [];
    public IEnumerator<IObserver<T>> GetEnumerator() => Observers.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Subscribe(IObserver<T> observer)
    {
        if (Observers.Contains(observer))
            return;

        Observers.Add(observer);
        logger.LogDebug("Observer subscribed");
    }

    public void Unsubscribe(IObserver<T> observer)
    {
        if (!Observers.Contains(observer))
            return;

        Observers.Remove(observer);
        logger.LogDebug("Observer unsubscribed");
    }

    public void Notify(T value)
    {
        var defunctObservers = new List<IObserver<T>>();

        foreach (var observer in Observers)
        {
            try
            {
                observer.OnNext(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error notifying observer");
                defunctObservers.Add(observer);
            }
        }

        // Remove any observers that errored out
        defunctObservers.ForEach(observer => Unsubscribe(observer));
    }
}
