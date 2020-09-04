using System;

namespace JuniorTennis.SeedWork.Exceptions
{
    [System.Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
    public class NotFoundException : Exception
    {
        public string Key { get; }

        public Type Type { get; }

        protected NotFoundException() : base() { }
        public NotFoundException(string key, Type type) : base()
        {
            this.Key = key;
            this.Type = type;
        }

        public NotFoundException(string key, Type type, string message) : base(message)
        {
            this.Key = key;
            this.Type = type;
        }

        public NotFoundException(string key, Type type, string message, Exception inner) : base(message, inner)
        {
            this.Key = key;
            this.Type = type;
        }

        protected NotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
