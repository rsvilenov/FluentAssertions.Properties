namespace FluentAssertions.Properties.Tests.Common
{
    public class AssertReason
    {
        public string Because { get; set; }
        public string BecauseWithFormat => $"{Because}{{0}}{{1}}";
        public string BecauseArg1 { get; set; }
        public string BecauseArg2 { get; set; }

        public override string ToString()
        {
            return string.Format(BecauseWithFormat, BecauseArg1, BecauseArg2);
        }
    }
}
