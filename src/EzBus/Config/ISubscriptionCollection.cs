using System.Collections;

namespace EzBus.Config
{
    public interface ISubscriptionCollection : ICollection
    {
        ISubscription this[int index] { get; set; }
    }
}