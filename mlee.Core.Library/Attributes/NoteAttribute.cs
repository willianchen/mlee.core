using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel.Description
{
    /// <summary>
    /// 自定义其他描述
    /// </summary>
    public class NoteAttribute : Attribute
    {
        public string Note { get; set; }

        public NoteAttribute(string note)
        {
            this.Note = note;
        }
    }
}
