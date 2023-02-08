using mlee.Core.Library.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace mlee.Core.Library.Dto
{
    public class Pager 
    {
        /// <summary>
        /// 初始化分页
        /// </summary>
        public Pager()
            : this(1)
        {
        }

        /// <summary>
        /// 初始化分页
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数,默认20</param> 
        /// <param name="totalCount">总行数</param>
        public Pager(int page, int pageSize = 20, int totalCount = 0)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        private int _pageIndex;

        /// <summary>
        /// 页索引，即第几页，从1开始
        /// </summary>
        public int Page
        {
            get
            {
                if (_pageIndex <= 0)
                    _pageIndex = 1;
                return _pageIndex;
            }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalCount == 0)
                    return 0;
                if ((TotalCount % PageSize) == 0)
                    return TotalCount / PageSize;
                return (TotalCount / PageSize) + 1;
            }
        }

        /// <summary>
        /// 跳过的行数
        /// </summary>
        public int SkipCount
        {
            get
            {
                if (Page > PageCount)
                    Page = PageCount;
                return PageSize * (Page - 1);
            }
        }

        public bool HasPrev => _pageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext => PageCount == 0 ? false : _pageIndex < PageCount;

        /// <summary>
        /// 排序 desc  asc
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderByField { get; set; }
    }
}