using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAccLogger
{
    public class FilterValues
    {
        /// <summary>
        /// this string can be substitued to any part of the log item filter key for accepting any value
        /// </summary>
        public const string AnyStringValueWildcard = "*";

        /// <summary>
        /// this object can be substitued to any object value for accepting any value
        /// </summary>
        public static readonly object AnyObjectValueWildcard = new object();

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
                            Dictionary<string, bool>>>>>
                EnabledFilters =
                    new Dictionary<object, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>>();

        /// <summary>
        /// clear filter values
        /// </summary>
        public void Clear()
        {
            EnabledFilters.Clear();
        }

        /// <summary>
        /// set values of a filter : (caller,callerTypeName,callerMemberName,logType,logCategroy) -+ isEnabled
        /// </summary>
        /// <param name="isEnabled">enabled (true), disabled (false)</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>
        public void AddOrSetValue(
            bool isEnabled,
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
            var klogType = (logType!=null)? logType.ToString() : AnyStringValueWildcard;
            var klogCategory = (logCategory != null)? logCategory.ToString() : AnyStringValueWildcard;

            if (!EnabledFilters.TryGetValue(kcaller, out var d1))
                EnabledFilters.Add(kcaller, d1 = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>() );
            if (!d1.TryGetValue(klogType, out var d2))
                d1.Add(klogType, d2 = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>() );
            if (!d2.TryGetValue(klogCategory, out var d3))
                d2.Add(klogCategory, d3 = new Dictionary<string, Dictionary<string, bool>>() );
            if (!d3.TryGetValue(kcallerTypeName, out var d4))
                d3.Add(kcallerTypeName, d4 = new Dictionary<string, bool>());
            if (!d4.ContainsKey(kcallerMemberName))
                d4.Add(kcallerMemberName, isEnabled);
            else
                d4[kcallerMemberName] = isEnabled;
        }

        /// <summary>
        /// return isEnabled value of a filter : (caller,callerTypeName,callerMemberName,logType,logCategroy) -+ isEnabled
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>
        /// <returns>filter isEnabled value if defined, else false</returns>
        public bool GetValue(
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

            if (!EnabledFilters.TryGetValue(kcaller, out var d1)
                && !EnabledFilters.TryGetValue(AnyObjectValueWildcard, out d1))
                return false;
            if (!d1.TryGetValue(klogType, out var d2)
                && !d1.TryGetValue(AnyStringValueWildcard, out d2))
                return false;
            if (!d2.TryGetValue(klogCategory, out var d3)
                && !d2.TryGetValue(AnyStringValueWildcard,out d3))
                return false;
            if (!d3.TryGetValue(kcallerTypeName, out var d4)
                && !d3.TryGetValue(AnyStringValueWildcard,out d4))
                return false;
            if (d4.TryGetValue(kcallerMemberName, out var r)
                || d4.TryGetValue(AnyStringValueWildcard, out r)
                )
                return r;
            return false;
        }
    }
}
