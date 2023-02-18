namespace ConsoleTests
{
    internal class RefType : IComparable<RefType>, ICloneable
    {
        public uint Prop { get; set; }
        public RefType() 
        {
            Prop = 73;
        }

        public int CompareTo(RefType? other)
        {
            return Prop.CompareTo(other?.Prop);
        }

        public object Clone()
        {
            return new RefType() { Prop = Prop };
        }
    }
}
