using Akka.Actor;
using System;

namespace Neo.IO.Actors
{
    internal class EventWrapper<T> : UntypedActor
    {
        private readonly Action<T> callback;

        public EventWrapper(Action<T> callback)
        {
            this.callback = callback;
        }

        protected override void OnReceive(object message)
        {
            if (message is T obj) callback(obj);
        }

        public static Props Props(Action<T> callback)
        {
            return Akka.Actor.Props.Create(() => new EventWrapper<T>(callback));
        }
    }
}
