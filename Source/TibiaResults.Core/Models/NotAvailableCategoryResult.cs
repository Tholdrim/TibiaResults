namespace TibiaResults.Core
{
    internal record NotAvailableCategoryResult() : CategoryResult(Array.Empty<CategoryResultEntry>())
    {
        public override bool IsAvailable => false;
    }
}
