namespace UniversityUnitTests.MocksAndFakes
{
    internal class TestReferenceType : IComparable<TestReferenceType>, ICloneable
    {
        public string Name { get; set; }

        public TestReferenceType()
        {
            Name = string.Empty;
        }

        public object Clone()
        {
            return new TestReferenceType() { Name = Name };
        }

        public int CompareTo(TestReferenceType? other)
        {
            if (other == null)
            {
                return -1;
            }
            return Name.CompareTo(other.Name);
        }
    }
}
