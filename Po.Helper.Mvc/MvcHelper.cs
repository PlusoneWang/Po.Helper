namespace Po.Helper.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// 用於MVC專案的輔助類別
    /// </summary>
    public static class MvcHelper
    {
        /// <summary>
        /// 轉換<![CDATA[IEnumberable<T>]]>泛型集合物件為<![CDATA[IEnumberable<SelectListItem>]]>選單集合物件
        /// </summary>
        /// <typeparam name="T"> 泛型類別 </typeparam>
        /// <param name="itemList"> 要轉換的集合物件 </param>
        /// <param name="includeItems">
        /// 委派，表示傳入的集合物件的個體要如何決定哪些項目要加入至SelectListItems
        /// <example> user => user != null </example>
        /// <example> user => true </example>
        /// </param>
        /// <param name="value">
        /// 委派，表示傳入的集合物件的個體要如何取得SelectListItem的Value值
        /// <example> user => user.Id.ToString() </example>
        /// </param>
        /// <param name="text">
        /// 委派，表示傳入的集合物件的個體要如何取得SelectListItem的Text值
        /// <example> user => user.Name </example>
        /// </param>
        /// <param name="selected">
        /// 委派，表示傳入的集合物件的個體要如何決定是否為Selected
        /// <example> user => user.Name == "Plusone" </example>
        /// <example> user => false </example>
        /// </param>
        /// <param name="addFirst">要加入至選擇清單首位的項目，一般用於預設項目，若為Null則不加入</param>
        /// <returns>選單集合物件</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(
            this IEnumerable<T> itemList,
            Func<T, bool> includeItems,
            Func<T, string> value,
            Func<T, string> text,
            Func<T, bool> selected,
            SelectListItem addFirst = null)
        {
            var selectListItems = itemList.Where(includeItems).Select(item => new SelectListItem { Value = value(item), Text = text(item), Selected = selected(item) }).ToList();
            if (addFirst != null)
            {
                selectListItems.Insert(0, addFirst);
            }

            return selectListItems;
        }
    }
}
