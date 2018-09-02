using System.Collections.Generic;

namespace NAccLogger
{
    /// <summary>
    /// log item properties filters. Associate a T value to a combination of property values and wildcards
    /// </summary>
    /// <typeparam name="T">type of filter values</typeparam>
    public class FilterValues<T>
    {
        /// <summary>
        /// this string can be substitued to any part of the log item filter key for accepting any value
        /// </summary>
        public readonly string AnyStringValueWildcard = "*";

        /// <summary>
        /// this object can be substitued to any object value for accepting any value
        /// </summary>
        public readonly object AnyObjectValueWildcard = new object();

        /// <summary>
        /// enabled/disabled log items for key combination
        /// <para>key ::= 
        /// LogType 
        /// -+ Caller
        /// -+ LogType
        /// -+ LogCategory 
        /// -+ CallerTypeName 
        /// -+ CallerMemberName</para>
        /// </summary>
        protected
            Dictionary<object,
                Dictionary<string,
                    Dictionary<string,
                        Dictionary<string,
                            Dictionary<string, T>>>>>
                Filters =
                    new Dictionary<object, 
                        Dictionary<string, 
                            Dictionary<string, 
                                Dictionary<string, 
                                    Dictionary<string, T>>>>>();

        /// <summary>
        /// clear filter values
        /// </summary>
        public void Clear()
        {
            Filters.Clear();
        }

        /// <summary>
        /// set values of a filter : (caller,callerTypeName,callerMemberName,logType,logCategroy) -+ isEnabled
        /// </summary>
        /// <param name="value">filter value of type T</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>
        public void AddOrSetValue(
            T value,
            object caller = null,
            string callerTypeName = null,
            string callerMemberName = null,
            LogType? logType = null,
            LogCategory? logCategory = null
            )
        {
            var kcaller = caller ?? AnyObjectValueWildcard;
            var kcallerTypeName = callerTypeName ?? AnyStringValueWildcard;
            var kcallerMemberName = callerMemberName ?? AnyStringValueWildcard;
            var klogType = (logType.HasValue)? logType.Value.ToString() : AnyStringValueWildcard;
            var klogCategory = (logCategory.HasValue)? logCategory.Value.ToString() : AnyStringValueWildcard;

            if (!Filters.TryGetValue(kcaller, out var d1))
                Filters.Add(kcaller, d1 = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, T>>>>() );
            if (!d1.TryGetValue(klogType, out var d2))
                d1.Add(klogType, d2 = new Dictionary<string, Dictionary<string, Dictionary<string, T>>>() );
            if (!d2.TryGetValue(klogCategory, out var d3))
                d2.Add(klogCategory, d3 = new Dictionary<string, Dictionary<string, T>>() );
            if (!d3.TryGetValue(kcallerTypeName, out var d4))
                d3.Add(kcallerTypeName, d4 = new Dictionary<string, T>());
            if (!d4.ContainsKey(kcallerMemberName))
                d4.Add(kcallerMemberName, value);
            else
                d4[kcallerMemberName] = value;
        }

        /// <summary>
        /// return a value matching filter properties : (caller,callerTypeName,callerMemberName,logType,logCategroy) -+ value
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>
        /// <returns>filter value if defined, else default(T)</returns>
        public T GetValue(
            object caller = null,
            string callerTypeName = null,
            string callerMemberName = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined
            )
        {
            var kcaller = caller ?? AnyObjectValueWildcard;
            var kcallerTypeName = callerTypeName ?? AnyStringValueWildcard;
            var kcallerMemberName = callerMemberName ?? AnyStringValueWildcard;
            var klogType = logType.ToString();
            var klogCategory = logCategory.ToString();

            if (!Filters.TryGetValue(kcaller, out var d1)
                && !Filters.TryGetValue(AnyObjectValueWildcard, out d1))
                return default(T);
            if (!d1.TryGetValue(klogType, out var d2)
                && !d1.TryGetValue(AnyStringValueWildcard, out d2))
                return default(T);
            if (!d2.TryGetValue(klogCategory, out var d3)
                && !d2.TryGetValue(AnyStringValueWildcard,out d3))
                return default(T);
            if (!d3.TryGetValue(kcallerTypeName, out var d4)
                && !d3.TryGetValue(AnyStringValueWildcard,out d4))
                return default(T);
            if (d4.TryGetValue(kcallerMemberName, out var r)
                || d4.TryGetValue(AnyStringValueWildcard, out r)
                )
                return r;
            return default(T);
        }
    }
}
