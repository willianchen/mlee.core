using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace mlee.Core.Library.Helpers
{
    /// <summary>
    /// Excel 帮助类
    /// 导出支持接收三种数据源  DataTable,Json 字符串,List<T>
    /// 导入支持返回三种数据源  DataTable,Json 字符串,List<T>
    /// </summary>
    public static partial class ExcelHelper
    {
        #region 导出

        /// <summary>
        /// 导出Excel
        /// 在指定路径中创建或覆盖文件。
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="param">导出参数</param>
        /// <returns></returns>
        public static async Task<ExcelMessage> ExportExcel(DataTable dt, ExportParam param)
        {
            var result = await Task.Run(async () =>
            {
                try
                {
                    int rowIndex = 0;
                    //创建Work
                    var wb = new HSSFWorkbook();

                    //if (param.ExcelType == ExcelType.XLS)
                    //{
                    //    wb = new HSSFWorkbook();
                    //}
                    //else
                    //{
                    //    wb = new XSSFWorkbook();
                    //}


                    if (wb == null) return new ExcelMessage { Success = false, Message = "文档格式不正确." };
                    int columnsCount = dt.Columns.Count;
                    //创建Sheet
                    ISheet sheet = InitSheetCreate(wb, columnsCount, param, ref rowIndex);
                    if (sheet == null) return new ExcelMessage { Success = false, Message = "Sheet初始化失败." };
                    IRow row = sheet.CreateRow(rowIndex);
                    ICellStyle style = InitCellStyle(wb);
                    //创建列：表头
                    for (int i = 0; i < columnsCount; i++)
                    {
                        row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                        row.GetCell(i).CellStyle = style;
                    }
                    //填充数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = sheet.CreateRow(i + rowIndex + 1);
                        for (int k = 0; k < columnsCount; k++)
                        {
                            row.CreateCell(k).SetCellValue(dt.Rows[i][k].ToString());
                        }
                    }
                    await SetStatistical(dt, param.Statistical, rowIndex, wb, sheet);

                    //var path = @".\Temp\" + DateTime.Now.Month;
                    //Directory.CreateDirectory(path);
                    //var savePath = path + "\\" + Guid.NewGuid() + (param.ExcelType == ExcelType.XLS ? ".xls" : ".xlsx");
                    ////导出Excel为临时文件
                    //using (FileStream stream = System.IO.File.Create(savePath))
                    //{
                    //    wb.Write(stream);
                    //}


                    ////将文件转为字节流
                    //FileStream fileStream = new FileStream(savePath, FileMode.Open);
                    //byte[] data = new byte[fileStream.Length];
                    //await fileStream.ReadAsync(data, 0, data.Length);
                    //fileStream.Close();

                    ////删除临时文件
                    //Directory.Delete(path, true);


                    MemoryStream ms = new MemoryStream();
                    wb.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] data = new byte[ms.Length];
                    await ms.ReadAsync(data, 0, data.Length);
                    await ms.DisposeAsync();
                    
                    return new ExcelMessage { Success = true, Data = data };
                }
                catch (Exception ex)
                {
                    return new ExcelMessage { Success = false, Message = "导出失败." + ex.Message };
                }
            });
            return result;
        }

    
       
        /// <summary>
        /// 新的Excel创建Sheet
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="columnsCount"></param>
        /// <param name="param"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        static ISheet InitSheetCreate(IWorkbook wb, int columnsCount, ExportParam param, ref int rowCount, bool IsModfiy = false, int ModfiySheetIndex = 0)
        {
            ISheet sheet = null;
            if (!IsModfiy)
                sheet = wb.CreateSheet(string.IsNullOrEmpty(param.SheetTitle) ? "Sheet1" : param.SheetTitle);
            else
                sheet = wb.GetSheetAt(ModfiySheetIndex);
            if (columnsCount < 6)
            {
                for (int i = 0; i < columnsCount; i++)
                {
                    sheet.SetColumnWidth(i, 5000);
                }
            }
            else
            {
                for (int i = 0; i < columnsCount; i++)
                {
                    sheet.SetColumnWidth(i, 3000);
                }
            }
            rowCount = 0;
            //创建行：
            //第一行：大标题
            {
                ICellStyle titleStyle = wb.CreateCellStyle();
                IFont titleFont = wb.CreateFont();
                titleFont.FontHeightInPoints = 14;//设置字体大小
                titleFont.Color = HSSFColor.Black.Index;//设置字体颜色
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;//居中
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 0, columnsCount - 1));
                sheet.CreateRow(rowCount).CreateCell(0).SetCellValue(param.Title);
                sheet.GetRow(rowCount).GetCell(0).CellStyle = titleStyle;
                rowCount++;
            }
            //第二行：副标题
            {
                ICellStyle titleStyle = wb.CreateCellStyle();
                IFont titleFont = wb.CreateFont();
                titleFont.FontHeightInPoints = 10;//设置字体大小
                titleFont.Color = HSSFColor.Black.Index;//设置字体颜色
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;//居中
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 0, columnsCount - 1));
                sheet.CreateRow(rowCount).CreateCell(0).SetCellValue(param.CompanyName + "      制表人:" + param.CreateName + "      制表时间:" + DateTime.Now);
                sheet.GetRow(rowCount).GetCell(0).CellStyle = titleStyle;
                rowCount++;
            }
            //第三行：扩展行
            {
                if (!string.IsNullOrEmpty(param.ExtendRow))
                {
                    ICellStyle titleStyle = wb.CreateCellStyle();
                    IFont titleFont = wb.CreateFont();
                    titleFont.FontHeightInPoints = 10;//设置字体大小
                    titleFont.Color = HSSFColor.Black.Index;//设置字体颜色
                    titleStyle.SetFont(titleFont);
                    titleStyle.Alignment = HorizontalAlignment.Left;
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowCount, rowCount, 0, columnsCount - 1));
                    sheet.CreateRow(rowCount).CreateCell(0).SetCellValue(param.ExtendRow);
                    sheet.GetRow(rowCount).GetCell(0).CellStyle = titleStyle;
                    rowCount++;
                }
            }
            return sheet;
        }
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="wb"></param>
        /// <returns>成功返回‘导出成功’；错误返回错误原因</returns>
        static ICellStyle InitCellStyle(IWorkbook wb)
        {
            ICellStyle style = wb.CreateCellStyle();
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.FillForegroundColor = HSSFColor.Orange.Index;
            style.FillPattern = FillPattern.SolidForeground;
            //水平居中
            style.Alignment = HorizontalAlignment.CenterSelection;
            //垂直居中
            style.VerticalAlignment = VerticalAlignment.Center;
            return style;
        }

        /// <summary>
        /// 追加统计行
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="Statistical">需要统计的列及统计类型</param>
        /// <param name="rowIndex">追加到第几行</param>
        /// <param name="sheet">需要统计的sheet</param>
        async static Task SetStatistical(DataTable dt, Dictionary<string, StatisticalType> Statistical, int rowIndex, IWorkbook wb, ISheet sheet)
        {
            if (Statistical != null)
            {
                await Task.Run(() =>
                {
                    IRow statisticalRow = sheet.CreateRow(dt.Rows.Count + rowIndex + 1);
                    ICellStyle titleStyle = wb.CreateCellStyle();
                    IFont titleFont = wb.CreateFont();
                    titleFont.FontHeightInPoints = 10;//设置字体大小
                    titleFont.Color = HSSFColor.Green.Index;//设置字体颜色
                    titleStyle.SetFont(titleFont);
                    titleStyle.Alignment = HorizontalAlignment.Left;//居中
                    titleStyle.BorderTop = BorderStyle.Thin;
                    titleStyle.BorderBottom = BorderStyle.Thin;
                    titleStyle.BorderLeft = BorderStyle.Thin;
                    titleStyle.BorderRight = BorderStyle.Thin;
                    statisticalRow.CreateCell(0).SetCellValue("总计:");
                    statisticalRow.GetCell(0).CellStyle = titleStyle;
                    foreach (var item in Statistical)
                    {
                        EnumerableRowCollection<double> rowColl = null;
                        if (dt.Columns[item.Key] == null) continue;
                        switch (dt.Columns[item.Key].DataType.Name)
                        {
                            case "String":
                                rowColl = dt.AsEnumerable().Select(s => Convert.ToDouble(s.Field<string>(item.Key)));
                                break;
                            case "Double":
                                rowColl = dt.AsEnumerable().Select(s => s.Field<double>(item.Key));
                                break;
                            case "Single":
                                rowColl = dt.AsEnumerable().Select(s => Convert.ToDouble(s.Field<float>(item.Key)));
                                break;
                            case "Decimal":
                                rowColl = dt.AsEnumerable().Select(s => Convert.ToDouble(s.Field<decimal>(item.Key)));
                                break;
                            case "Int32":
                                rowColl = dt.AsEnumerable().Select(s => Convert.ToDouble(s.Field<int>(item.Key)));
                                break;
                            default:
                                break;
                        }
                        if (rowColl == null) continue;
                        double result = 0.0;
                        switch (item.Value)
                        {
                            case StatisticalType.SUM:
                                result = rowColl.Sum();
                                statisticalRow.CreateCell(dt.Columns[item.Key].Ordinal).SetCellValue(result);
                                statisticalRow.GetCell(dt.Columns[item.Key].Ordinal).CellStyle = titleStyle;
                                break;
                            case StatisticalType.AVG:
                                result = rowColl.Average();
                                statisticalRow.CreateCell(dt.Columns[item.Key].Ordinal).SetCellValue(result);
                                statisticalRow.GetCell(dt.Columns[item.Key].Ordinal).CellStyle = titleStyle;
                                break;
                            case StatisticalType.MAX:
                                result = rowColl.Max();
                                statisticalRow.CreateCell(dt.Columns[item.Key].Ordinal).SetCellValue(result);
                                statisticalRow.GetCell(dt.Columns[item.Key].Ordinal).CellStyle = titleStyle;
                                break;
                            case StatisticalType.MIN:
                                result = rowColl.Min();
                                statisticalRow.CreateCell(dt.Columns[item.Key].Ordinal).SetCellValue(result);
                                statisticalRow.GetCell(dt.Columns[item.Key].Ordinal).CellStyle = titleStyle;
                                break;
                            case StatisticalType.COUNT:
                                statisticalRow.CreateCell(dt.Columns[item.Key].Ordinal).SetCellValue(dt.Rows.Count);
                                statisticalRow.GetCell(dt.Columns[item.Key].Ordinal).CellStyle = titleStyle;
                                break;
                            default:
                                break;
                        }
                    }
                });
            }
        }


        #region 现有Excel表格基础上导出

        ///// <summary>
        ///// 现有Excel表格上导出数据(根据title新增sheet)
        ///// </summary>
        ///// <param name="dt">DataTable数据源</param>
        ///// <param name="path">文件路径</param>
        ///// <param name="sheetTitle">子表名称，存在则覆盖，不存在则创建</param>
        ///// <returns>成功返回True ，失败返回 False</returns>
        //public static async Task<ExcelMessage> ExportExcel_Additional_Sheet(DataTable dt, string path, string sheetTitle)
        //{
        //    var result = await Task.Run(() =>
        //    {
        //        try
        //        {
        //            IWorkbook wb = InitWorkOpen(path);
        //            if (wb == null) return new ExcelMessage { Success = false, Message = "文档格式不正确." };
        //            //获取现有Sheet
        //            ISheet sheet = InitSheetOpen(wb, sheetTitle);
        //            if (sheet == null) return new ExcelMessage { Success = false, Message = "Sheet初始化失败." };
        //            //创建行：首行
        //            IRow row = sheet.CreateRow(0);
        //            int columnsCount = dt.Columns.Count;
        //            //创建列：表头
        //            for (int i = 0; i < columnsCount; i++)
        //            {
        //                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
        //            }
        //            //填充数据
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                row = sheet.CreateRow(i + 1);
        //                for (int k = 0; k < columnsCount; k++)
        //                {
        //                    row.CreateCell(k).SetCellValue(dt.Rows[i][k].ToString());
        //                }
        //            }
        //            //导出Excel
        //            using (FileStream stream = System.IO.File.Create(path))
        //            {
        //                wb.Write(stream);
        //            }
        //            return new ExcelMessage { Success = true };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ExcelMessage { Success = false, Message = "导出失败." + ex.Message };
        //        }
        //    });

        //    return result;
        //}

        ///// <summary>
        ///// 向现有Excel 表格追加头部（即大标题，副标题，扩展行）
        ///// 需要追加统计行时，需要提供原始 DataTable 数据源
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="param"></param>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public static async Task<ExcelMessage> ExportExcel_Additional_HeaderStatistics(ExportParam param, DataTable dt = null)
        //{
        //    var result = await Task.Run(async () =>
        //    {
        //        try
        //        {
        //            IWorkbook wb = InitWorkOpen(param.Path);
        //            if (wb == null) return new ExcelMessage { Success = false, Message = "文档格式不正确." };
        //            //获取现有Sheet
        //            ISheet sheet = wb.GetSheetAt(0);
        //            if (sheet == null) return new ExcelMessage { Success = false, Message = "Sheet初始化失败." };
        //            int rowcount = sheet.LastRowNum;
        //            sheet.ShiftRows(0, rowcount, string.IsNullOrEmpty(param.ExtendRow) ? 2 : 3);
        //            rowcount = sheet.LastRowNum;
        //            sheet = InitSheetCreate(wb, sheet.GetRow((string.IsNullOrEmpty(param.ExtendRow) ? 2 : 3) + 1).LastCellNum, param, ref rowcount, true);
        //            if (dt != null)
        //                await SetStatistical(dt, param.Statistical, rowcount, wb, sheet);
        //            //导出Excel
        //            using (FileStream stream = System.IO.File.Create(param.Path))
        //            {
        //                wb.Write(stream);
        //            }
        //            return new ExcelMessage { Success = true };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ExcelMessage { Success = false, Message = "导出失败." + ex.Message };
        //        }
        //    });

        //    return result;         
        //}

        ///// <summary>
        ///// 打开Excel
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //static IWorkbook InitWorkOpen(string path)
        //{
        //    IWorkbook wb = null;
        //    string excelType = Path.GetExtension(path);
        //    if (excelType == ".xls")
        //    {
        //        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            wb = new HSSFWorkbook(fs);
        //        }
        //    }
        //    else if (excelType == ".xlsx")
        //    {
        //        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            wb = new XSSFWorkbook(fs);
        //        }
        //    }
        //    return wb;
        //}

        ///// <summary>
        ///// 现有Excel表格上新增或覆盖sheet
        ///// </summary>
        ///// <param name="wb"></param>
        ///// <param name="sheetTitle"></param>
        ///// <returns></returns>
        //static ISheet InitSheetOpen(IWorkbook wb, string sheetTitle)
        //{
        //    ISheet sheet = wb.GetSheet(string.IsNullOrEmpty(sheetTitle) == true ? DateTime.Now.ToString("yyyyMMddhhmmssfff") : sheetTitle);
        //    if (sheet == null)
        //    {
        //        //创建新的Sheet
        //        sheet = wb.CreateSheet(string.IsNullOrEmpty(sheetTitle) == true ? DateTime.Now.ToString("yyyyMMddhhmmssfff") : sheetTitle);
        //    }
        //    return sheet;
        //}

       
        #endregion

        #endregion

        #region 导入
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="cellName">可空，Key:原列标题，Value:新列表标题</param>
        /// <param name="sheetTitle">子表名称，默认为空，读取第一张表</param>
        /// <returns>列名为NULL时返回完整Excel表格，反之返回指定列名的 DataTable </returns>
        public static DataTable ImportExcel_DataTable(string path, Dictionary<string, string> cellName = null, string sheetTitle = "")
        {
            return GetDataTable(path, cellName, sheetTitle);
        }
        /// <summary>
        ///  导入Excel
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="cellName">可空，Key:原列标题，Value:新列表标题</param>
        /// <param name="sheetTitle">子表名称，默认为空，读取第一张表</param>
        /// <returns>列名为NULL时返回完整Excel表格，反之返回指定列名的  JSON 字符串</returns>
        public static string ImportExcel_Json(string path, Dictionary<string, string> cellName = null, string sheetTitle = "")
        {
            string json = "";
            DataTable dt = GetDataTable(path, cellName, sheetTitle);
            if (dt == null) return "";
            json = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json += "{";
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    json += "\"" + dt.Columns[k].ColumnName + "\":" + "\"" + dt.Rows[i][dt.Columns[k].ColumnName] + "\",";
                }
                json += "},";
            }
            json += "]";
            return json;
        }
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="path">路径</param>
        /// <param name="cellName">必填，Key:原列标题，Value:实体类属性名</param>
        /// <param name="sheetTitle">子表名称，默认为空，读取第一张表</param>
        /// <returns>泛型集合</returns>
        public static List<T> ImportExcel_List<T>(string path, Dictionary<string, string> cellName, string sheetTitle = "") where T : class, new()
        {
            DataTable dt = GetDataTable(path, cellName, sheetTitle);
            if (dt == null) return null;
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!System.Convert.IsDBNull(item[i]))
                            {
                                dynamic v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Int32")
                                        v = ((string)item[i]).ToInt();
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.DateTime")
                                        v = ((string)item[i]).ToDateTime();
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Boolean")
                                        v = ((string)item[i]).ToBool();
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Byte")
                                        v = ((string)item[i]).ToBytes();
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Double")
                                        v = ((string)item[i]).ToDouble();
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Single")
                                         v = System.Convert.ToSingle((string)item[i]);
                                    else if (Nullable.GetUnderlyingType(info.PropertyType).ToString() == "System.Decimal")
                                        v = ((string)item[i]).ToDecimal();
                                    else
                                        v = (string)item[i];
                                }
                                else
                                {
                                    v = System.Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="cellName">Key:原列标题，Value:新列表标题</param>
        /// <param name="sheetTitle">子表名称，默认为空，读取第一张表</param>
        /// <returns></returns>
        private static DataTable GetDataTable(string path, Dictionary<string, string> cellName, string sheetTitle = "")
        {
            DataTable dt = new DataTable();
            try
            {
                IWorkbook wb = null;
                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    string excelType = Path.GetExtension(path);
                    if (excelType == ".xls")
                    {
                        wb = new HSSFWorkbook(stream);
                    }
                    else if (excelType == ".xlsx")
                    {
                        wb = new XSSFWorkbook(stream);
                    }
                    else
                        return null;
                }
                if (wb.NumberOfSheets <= 0) return null;
                ISheet sheet = string.IsNullOrEmpty(sheetTitle) ? wb.GetSheetAt(0) : wb.GetSheet(sheetTitle);
                IRow row = sheet.GetRow(0);
                int rowCount = sheet.LastRowNum;
                string columnName = "";
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    string column = row.GetCell(i).StringCellValue;
                    if (string.IsNullOrEmpty(column)) continue; //过滤空表头
                    columnName += column + ",";
                    dt.Columns.Add(column, typeof(string));
                }
                string[] cells = columnName.TrimEnd(',').Split(',');
                if (cellName != null)
                {
                    string CellNames = "";
                    foreach (var item in cellName)
                    {
                        CellNames += item.Key + ",";
                    }
                    CellNames = CellNames.TrimEnd(',');
                    var list = CellNames.Split(',').Intersect(cells);
                    if (list.Intersect(CellNames.Split(',')).Count() != CellNames.Split(',').Count())
                    {
                        return null;
                    }
                }
                #region 根据Excel列头生成DataTable
                for (int i = 1; i <= rowCount; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null) continue;
                    DataRow dataRow = dt.NewRow();
                    bool tag = false;
                    for (int k = 0; k < cells.Length; k++)
                    {
                        dataRow[k] = row.GetCell(k) == null ? System.Convert.DBNull : row.GetCell(k).ToString();
                        if (dataRow[k].ToString() != "")
                            tag = true;
                    }
                    if (tag)//过滤空行
                        dt.Rows.Add(dataRow);
                }
                #endregion 
                #region 根据指定列名生成DataTable
                if (cellName != null)
                {
                    DataTable data = new DataTable();
                    foreach (var item in cellName)
                    {
                        data.Columns.Add(cellName[item.Key], typeof(string));
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dataRow = data.NewRow();
                        int k = 0;
                        foreach (var item in cellName)
                        {
                            dataRow[k] = dt.Rows[i][item.Key];
                            k++;
                        }
                        data.Rows.Add(dataRow);
                    }
                    return data;
                }
                #endregion
                return dt;
            }
            catch (Exception EX)
            {
                return null;
            }
        }
        #endregion

    }
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticalType
    {
        SUM,
        AVG,
        MAX,
        MIN,
        COUNT,
    }
    /// <summary>
    /// 导出参数
    /// </summary>
    public class ExportParam
    {
        public ExportParam(string Title, ExcelType ExcelType = ExcelType.XLS, Dictionary<string, StatisticalType> Statistical = null)
        {
            this.Title = Title;
            this.ExcelType = ExcelType;
            this.Statistical = Statistical;
            CreateName = "Admin";
            CompanyName = "杭州猎板网络科技有限公司";
            SheetTitle = "Sheet1";
            ExtendRow = "";
        }
        /// <summary>
        /// 大标题：报表名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 需要统计的行可为null，Key:需要统计的列,value:统计类型
        /// </summary>
        public Dictionary<string, StatisticalType> Statistical { get; set; }
        /// <summary>
        /// 制表人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Sheet名称
        /// </summary>
        public string SheetTitle { get; set; }
        /// <summary>
        /// 扩展行，
        /// </summary>
        public string ExtendRow { get; set; }

        /// <summary>
        /// 导出类型
        /// </summary>
        public ExcelType ExcelType { get; set; }
    }


    /// <summary>
    /// 导出类型
    /// </summary>
    public enum ExcelType
    {
        XLS,
        //XLSX
    }

    /// <summary>
    /// 返回消息
    /// </summary>
    public class ExcelMessage {

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string  Message { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] Data { get; set; }
    }
}
