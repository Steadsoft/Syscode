namespace Syscode.Ast
{
    public class Alignment
    {
        private AlignmentUnits alignmentUnits;
        private int alignmentValue;

        public Alignment()
        {
            AlignmentUnits = AlignmentUnits.Unspecified;
            AlignmentValue = 0;
        }

        public AlignmentUnits AlignmentUnits { get => alignmentUnits; set => alignmentUnits = value; }
        public int AlignmentValue { get => alignmentValue; set => alignmentValue = value; }
    }

    public enum AlignmentUnits
    {
        Unspecified,
        Bits,
        Bytes
    }
}
