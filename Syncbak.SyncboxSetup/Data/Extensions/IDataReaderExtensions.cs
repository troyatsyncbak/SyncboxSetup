using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Syncbak.SyncboxSetup.Data.Extensions
{
    public static class DataReaderExtensions
    {
        public static double ReadDouble(this IDataReader reader, string columnName, double defaultValue = 0.0, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return Convert.ToDouble(result);
        }

        public static int ReadInt(this IDataReader reader, string columnName, int defaultValue = 0, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return Convert.ToInt32(result);
        }

        public static byte ReadByte(this IDataReader reader, string columnName, byte defaultValue = 0, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return Convert.ToByte(result);
        }

        public static string ReadString(this IDataReader reader, string columnName, string defaultValue = null, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return Convert.ToString(result);
        }

        public static bool ReadBool(this IDataReader reader, string columnName, bool defaultValue = false, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return Convert.ToBoolean(result);
        }

        public static byte[] ReadByteArray(this IDataReader reader, string columnName, byte[] defaultValue = null, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            return (byte[])result;
        }

        public static DateTime ReadDateTime(this IDataReader reader, string columnName, DateTime defaultValue = default(DateTime), bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            var dt = (DateTime)result;

            return dt.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(dt, DateTimeKind.Utc) : dt;
        }

        public static DateTime? ReadNullableDateTime(this IDataReader reader, string columnName, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return null;

            var result = reader[columnName];

            if (result == DBNull.Value)
                return null;

            var dt = (DateTime?) result;

            if (dt.HasValue && dt.Value.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);

            return (DateTime?)result;
        }

        public static DateTimeOffset ReadDateTimeOffset(this IDataReader reader, string columnName, DateTimeOffset defaultValue = default(DateTimeOffset), bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            // Handle the conversion from DateTime to DateTimeOffset
            if (!(result is DateTimeOffset) && (result is DateTime))
            {
                var dt = (DateTime)result;
                switch (dt.Kind)
                {
                    case DateTimeKind.Unspecified: // Treat unspecified as UTC
                        result = new DateTimeOffset(dt, TimeSpan.Zero);
                        break;
                    default:
                        result = new DateTimeOffset(dt);
                        break;
                }
            }

            return (DateTimeOffset)result;
        }

        public static DateTimeOffset? ReadNullableDateTimeOffset(this IDataReader reader, string columnName, bool enforceColumnExistance = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return null;

            var result = reader[columnName];

            if (result == DBNull.Value)
                return null;


            // Handle the conversion from DateTime to DateTimeOffset
            if (!(result is DateTimeOffset) && (result is DateTime))
            {
                var dt = (DateTime) result;
                switch(dt.Kind)
                {
                    case DateTimeKind.Unspecified: // Treat unspecified as UTC
                        result = new DateTimeOffset(dt, TimeSpan.Zero);
                        break;
                    default:
                        result = new DateTimeOffset(dt);
                        break;
                }
            }

            var debug = (DateTimeOffset?) result;

            return (DateTimeOffset?)result;
        }

        //Note: These do not currently work with enums

        public static T ParseValue<T>(this IDataReader reader, string columnName, bool enforceColumnExistance = true, bool enforceType = true)
        {
            return ParseValue(reader, columnName, default(T), enforceColumnExistance, enforceType);
        }

        public static T ParseValue<T>(this IDataReader reader, string columnName, T defaultValue, bool enforceColumnExistance = true, bool enforceType = true)
        {
            if (!enforceColumnExistance && !ColumnExists(reader, columnName))
                return defaultValue;

            var result = reader[columnName];

            if (result == null || result == DBNull.Value)
                return defaultValue;

            if (!enforceType && !(result is T))
                return defaultValue;

            return (T)result;
        }

        public static bool ColumnExists(IDataRecord reader, string columnName)
        {
            return !string.IsNullOrWhiteSpace(columnName) && GetColumnInfo(reader).Any(o => o.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase));
        }

        public static IEnumerable<string> GetColumnNames(IDataRecord reader)
        {
            return GetColumnInfo(reader).Select(o => o.ToString());
        }

        public static IEnumerable<ColumnInfo> GetColumnInfo(IDataRecord reader)
        {
            var result = new List<ColumnInfo>();

            if (reader != null)
                for (var i = 0; i < reader.FieldCount; i++)
                    result.Add(new ColumnInfo { Name = reader.GetName(i), Type = reader.GetFieldType(i) });

            return result;
        }

        public class ColumnInfo
        {
            public string Name { get; set; }
            public Type Type { get; set; }

            public override string ToString()
            {
                return string.Format("{0} ({1})", Name, Type);
            }
        }
    }
}
