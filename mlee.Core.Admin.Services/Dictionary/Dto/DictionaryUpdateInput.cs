using System.ComponentModel.DataAnnotations;
using mlee.Core.Library.Validations.Validators;

namespace mlee.Core.Services.Dictionary.Dto;

/// <summary>
/// 修改
/// </summary>
public class DictionaryUpdateInput : DictionaryAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择数据字典")]
    public long Id { get; set; }
}