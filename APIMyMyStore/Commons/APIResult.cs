using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace APIMyMyStore
{
    public class APIResult
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public void SetDataResult(DataSet pData)
        {
            List<List<object>> lstDataSet = new List<List<object>>();
            List<object> lstTables;
            string[] lstColumns;
            List<object> lstRows;
            List<object> itemRow;

            for (int i = 0; i < pData.Tables.Count; i++)
            {
                lstTables = new List<object>();
                lstColumns = pData.Tables[i].Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                lstTables.Add(lstColumns);
                lstRows = new List<object>();
                for (int row = 0; row < pData.Tables[i].Rows.Count; row++)
                {
                    itemRow = new List<object>();
                    for (int col = 0; col < pData.Tables[i].Columns.Count; col++)
                    {
                        if (pData.Tables[i].Rows[row][col] == DBNull.Value)
                        {
                            itemRow.Add(string.Empty);
                        }
                        else
                        {
                            itemRow.Add(pData.Tables[i].Rows[row][col]);
                        }
                    }
                    lstRows.Add(itemRow);
                }
                lstTables.Add(lstRows);
                lstDataSet.Add(lstTables);
            }
            Data = lstDataSet;
            Status = 1;
        }

        public void SetDataResult(Dictionary<string, object> pData)
        {
            List<List<object>> lstDataSet = new List<List<object>>();
            List<object> lstTables = new List<object>();
            List<object> lstColumns = new List<object>();
            List<object> lstRows = new List<object>();
            List<object> itemRow = new List<object>();

            foreach (string columnName in pData.Keys)
            {
                lstColumns.Add(columnName);
                itemRow.Add(pData[columnName]);
            }

            lstRows.Add(itemRow);
            lstTables.Add(lstColumns);
            lstTables.Add(lstRows);
            lstDataSet.Add(lstTables);

            Data = lstDataSet;
            Status = 1;
        }

        public void SetIntResult(long pValue)
        {
            Data = pValue;
            Status = 1;
        }

        public void SetResult(object pValue)
        {
            if (pValue is DataSet)
            {
                this.SetDataResult((DataSet)pValue); 
            }
            else if(pValue is Dictionary<string, object>)
            {
                this.SetDataResult((Dictionary<string, object>)pValue); 
            }
            else 
            {
                Data = pValue;
                Status = 1;
            }
        }

        public void SetResultFrontEnd(object pValue)
        {
            if (pValue is DataSet)
            {
                this.SetDataResult((DataSet)pValue); 
            }
            else if(pValue is DataTable) 
            {
                Data = CommonMethods.ConvertDataToList((DataTable)pValue);
                Status = 1;
            }
            else
            {
                Data = pValue;
                Status = 1;
            }
        }

        public void SetException(Exception pException)
        {
            Status = 0;
            Message = pException.Message;
        }
    }
}