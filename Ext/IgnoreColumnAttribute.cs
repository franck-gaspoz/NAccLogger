using System;

namespace NAccLogger.Ext
{
    /// <summary>
    /// indicates to a log item formatter to ignore a log item property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreColumnAttribute
        : Attribute
    {
        public IgnoreColumnAttribute()
        {

        }
    }
}
