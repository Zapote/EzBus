using System;

namespace EzBus
{
    public class EndpointAddress
    {
        public string Name { get; }
        public string MachineName { get; }

        public EndpointAddress(string name, string machineName = "")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name.ToLower();
            MachineName = machineName;
        }

        private bool Equals(EndpointAddress other)
        {
            return string.Equals(Name, other.Name) && string.Equals(MachineName, other.MachineName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EndpointAddress)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (MachineName != null ? MachineName.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(MachineName) ? Name : string.Format("{0}@{1}", Name, MachineName);
        }

        public static EndpointAddress Parse(string s)
        {
            var parts = s.Split('@');
            return parts.Length > 1 ? new EndpointAddress(parts[0], parts[1]) : new EndpointAddress(parts[0]);
        }
    }
}