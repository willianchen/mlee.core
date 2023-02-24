using System.ComponentModel.DataAnnotations;
using mlee.Core.Library.Validations.Validators;

namespace mlee.Core.Services.Document.Dto;

public class DocumentUpdateMenuInput : DocumentAddMenuInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择菜单")]
    public long Id { get; set; }
}