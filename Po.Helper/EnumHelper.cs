namespace Po.Helper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Enum的屬性存取輔助
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 取得列舉物件中指定的屬性
        /// </summary>
        /// <typeparam name="TAttribute">指定的屬性類型</typeparam>
        /// <param name="enumObj">列舉物件</param>
        /// <exception cref="NullReferenceException">型別轉換時發生錯誤</exception>
        /// <exception cref="Exception">Cannot find specified attribute type from enum object.</exception>
        /// <returns>指定的屬性物件</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumObj) where TAttribute : Attribute
        {
            var objName = enumObj.ToString();
            var type = enumObj.GetType();
            var fieldInfo = type.GetField(objName);

            if (!(fieldInfo.GetCustomAttributes(typeof(TAttribute), false) is TAttribute[] attributes))
                throw new NullReferenceException("型別轉換時發生錯誤");

            if (attributes.Length == 0)
                throw new Exception("Cannot find specified attribute type from enum object.");

            return attributes[0];
        }

        /// <summary>
        /// 取得列舉值對應之列舉物件中指定的屬性
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <typeparam name="TAttribute">指定的屬性類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>指定的屬性物件</returns>
        public static TAttribute GetAttribute<TEnum, TAttribute>(this int enumValue) where TEnum : Enum where TAttribute : Attribute
        {
            var e = Enum.Parse(typeof(TEnum), Convert.ToString(enumValue));

            return ((Enum)e).GetAttribute<TAttribute>();
        }

        /// <summary>
        /// 取得列舉物件的顯示名稱
        /// </summary>
        /// <param name="enumObj">列舉物件</param>
        /// <returns>
        /// 列舉顯示名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDisplayName(this Enum enumObj)
        {
            try
            {
                return GetAttribute<DisplayAttribute>(enumObj).Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的顯示名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉顯示名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDisplayName<TEnum>(this int enumValue) where TEnum : Enum
        {
            try
            {
                return GetAttribute<TEnum, DisplayAttribute>(enumValue).Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉物件的描述
        /// </summary>
        /// <param name="enumObj">列舉物件</param>
        /// <returns>
        /// 列舉描述
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDescription(this Enum enumObj)
        {
            try
            {
                return GetAttribute<DescriptionAttribute>(enumObj).Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的描述
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉描述
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDescription<TEnum>(this int enumValue) where TEnum : Enum
        {
            try
            {
                return GetAttribute<TEnum, DescriptionAttribute>(enumValue).Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉物件的短名稱
        /// </summary>
        /// <param name="enumObj">列舉物件</param>
        /// <returns>
        /// 列舉短名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetShortNames(this Enum enumObj)
        {
            try
            {
                return GetAttribute<DisplayAttribute>(enumObj).ShortName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的短名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉短名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetShortNames<TEnum>(this int enumValue) where TEnum : Enum
        {
            try
            {
                return GetAttribute<TEnum, DisplayAttribute>(enumValue).ShortName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉Property名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類別</typeparam>
        /// <param name="enumValue">列舉數字</param>
        /// <returns>列舉名稱</returns>
        public static string GetName<TEnum>(this int enumValue) where TEnum : Enum
        {
            var property = Enum.Parse(typeof(TEnum), Convert.ToString(enumValue));

            return property.ToString();
        }

        /// <summary>
        /// 使用自訂方法，依序對所有列舉物件執行方法，並取得方法回傳值所構成的<![CDATA[IEnumerable<T>]]>物件
        /// </summary>
        /// <typeparam name="T">IEnumerable的裝載型別</typeparam>
        /// <typeparam name="TEnum">列舉型別</typeparam>
        /// <param name="getT">自訂方法，表示要對每個列舉物件執行的動作</param>
        /// <returns>自訂方法回傳值所構成的<![CDATA[IEnumerable<T>]]>物件</returns>
        public static IEnumerable<T> AsIEnumerable<T, TEnum>(Func<Enum, T> getT) where TEnum : Enum
        {
            var result = new List<T>();
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                result.Add(getT((Enum)value));
            }

            return result;
        }
    }
}
