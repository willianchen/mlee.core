using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library
{
    /// <summary>
    /// 列表项
    /// </summary>
    public class Item : IComparable<Item>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        /// <param name="sortId">排序号</param>
        /// <param name="group">组</param>
        /// <param name="disabled">禁用</param>
        public Item(string text, object value, string name = "", int? sortId = null, string group = null, bool? disabled = null, long? parId = 0, int? childNum = 0, bool isChecked = false)
        {
            Text = text;
            Value = value;
            Name = name;
            SortId = sortId;
            Group = group;
            Disabled = disabled;
            ParId = parId;
            ChildNum = childNum;
            IsChecked = isChecked;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; }

        public object Name { get; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortId { get; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool? Disabled { get; }

        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParId { get; }

        /// <summary>
        /// 子节点数量
        /// </summary>
        public int? ChildNum { get; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; }


        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其它列表项</param>
        public int CompareTo(Item other)
        {
            return string.Compare(Text, other.Text, StringComparison.CurrentCulture);
        }
    }
}

