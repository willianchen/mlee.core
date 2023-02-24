using System.ComponentModel.DataAnnotations;
using mlee.Core.Library.Validations.Validators;

namespace mlee.Core.Services.DictionaryType.Dto;

/// <summary>
/// 修改
/// </summary>
public class DictionaryTypeUpdateInput : DictionaryTypeAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择数据字典类型")]
    public long Id { get; set; }
}