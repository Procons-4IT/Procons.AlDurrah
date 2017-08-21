namespace Procons.DataBaseHelper
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MappingEntityAttribute : Attribute
    {
        public MappingEntityAttribute()
        {

        }
    }
}
