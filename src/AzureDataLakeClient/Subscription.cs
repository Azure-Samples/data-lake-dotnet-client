using System;

namespace AdlClient
{
    public class Subscription
    {
        public readonly string Id;

        public Subscription(string id)
        {
            if (!System.Guid.TryParse(id, out Guid g))
            {
                throw new System.ArgumentException("id is not a valid guid");      
            }

            this.Id = id;
        }

        public Subscription(Guid id) :
            this(id.ToString())
        {
        }

    }
}