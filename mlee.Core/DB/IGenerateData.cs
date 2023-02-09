using mlee.Core.Library.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.DB
{

    /// <summary>
    /// 生成数据接口
    /// </summary>
    public interface IGenerateData
    {
        Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
    }

}
