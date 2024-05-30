
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Strongly typed string for navn
    /// </summary>
    public struct SimulationName : IEquatable<SimulationName>
    {
        private readonly string _name; 
        public SimulationName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidNameException("Name cannot be empty.");
            }
            _name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is SimulationName other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(SimulationName other)
        {
            return _name == other._name;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public static bool operator ==(SimulationName left, SimulationName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SimulationName left, SimulationName right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}