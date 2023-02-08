using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace mlee.Core.Library.Authentication
{
    public class HuntNetAuthOption : AuthenticationSchemeOptions
    {
        public HuntNetAuthOption()
            : base()
        {
            Events = new HuntNetEvents();
        }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
      
        public Func<string, string, bool> ValidateUser { get; set; }
        public new HuntNetEvents Events
        {
            get { return (HuntNetEvents)base.Events; }
            set { base.Events = value; }
        }
    }
}