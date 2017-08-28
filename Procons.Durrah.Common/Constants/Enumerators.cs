namespace Procons.Durrah.Common.Enumerators
{
    public enum AccountTypes
    {
        Debit,
        Credit
    }
    //Currently Supported Actions
    public enum DocActiontType
    {
        Close = 1,
        Cancel = 2
    }

    //The property types when formatting JSON in WCF client.
    public enum PropertyType
    {
        SimpleEdmx = 0,
        ComplexType = 1,
        Collection = 2              
    }

    public enum UpdateSemantics
    {
        PUT = 0,
        PATCH = 1
    }

    //public enum Operation
    //{
    //    EqualTo,
    //    NotEqualTo,
    //    GreaterThan,
    //    GreaterThanOrEqualTo,
    //    LessThan,
    //    LessThanOrEqualTo
    //}
}
