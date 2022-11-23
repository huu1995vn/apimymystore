using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace DBLibrary
{
    public class TemplateDAL : DBConnection
    {
        #region "Properties"

        public string TableName = string.Empty;
        public string ID = "Id";

        #endregion

        #region "Contructors"

        public TemplateDAL(string pConnection) : base(pConnection)
        {

        }

        public TemplateDAL(string pConnection, string pTableName) : base(pConnection)
        {
            this.TableName = pTableName;
        }

        #endregion

        #region "Sync Methods"

        #region "Get Data"

        public DataSet GetDataByInIds(string pSelect, long[] pIds)
        {
            if (pIds == null || pIds.Length == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return GetData(cmd);
        }

        public DataSet GetDataByInIds(string pSelect, List<long> pIds)
        {
            if (pIds == null || pIds.Count == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return GetData(cmd);
        }

        public DataSet GetDataByInIds(string pSelect, long[] pIds, string pCondition)
        {
            if (pIds == null || pIds.Length == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql = sql + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return GetData(cmd);
        }

        public DataSet GetDataByInIds(string pSelect, List<long> pIds, string pCondition)
        {
            if (pIds == null || pIds.Count == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql = sql + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return GetData(cmd);
        }

        public DataSet GetData(string pSQL, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
            if (pFields != null)
            {
                for (int i = 0; i < pFields.Length; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            return GetData(cmd);
        }

        public DataSet GetDataBy(string pSelect, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            cmd.CommandText = sql.ToString();
            return GetData(cmd);
        }

        public DataSet GetDataBy(string pSelect, string[] pFields, object[] pDatas, string pTextOrder)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return GetData(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSelect"></param>
        /// <param name="pCondition">Phai chua cac doi so cua pFields va pDatas</param>
        /// <param name="pTextOrder"></param>
        /// <param name="pFields"></param>
        /// <param name="pDatas"></param>
        /// <returns></returns>
        public DataSet GetDataBy(string pSelect, string pCondition, string pTextOrder, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                for (int i = 0; i < pFields.Length; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql.Append(" WHERE " + pCondition);
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return GetData(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSelect"></param>
        /// <param name="pFields"></param>
        /// <param name="pDatas"></param>
        /// <param name="pCondition">Khong chua cac doi so cua pFields va pDatas</param>
        /// <param name="pTextOrder"></param>
        /// <returns></returns>
        public DataSet GetDataBy(string pSelect, string[] pFields, object[] pDatas, string pCondition, string pTextOrder)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null && pFields.Length > 0)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (pFields == null || pFields.Length == 0)
                {
                    sql.Append(" WHERE ");
                }
                else
                {
                    sql.Append(" AND ");
                }
                sql.Append("(" + pCondition + ")");
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return GetData(cmd);
        }

        public DataSet GetDataBy(string pSelect, string[] pFields, object[] pDatas, string pCondition, string pTextOrder, bool pIsSearch)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null && pFields.Length > 0)
            {
                sql.Append(" WHERE (");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(pIsSearch ? " OR " : " AND ");
                    }
                    if (pIsSearch)
                    {
                        sql.Append(pFields[i] + " LIKE N'%'+@" + pFields[i] + "+'%'");
                    }
                    else
                    {
                        sql.Append(pFields[i] + "=@" + pFields[i]);
                    }
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
                sql.Append(")");
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (pFields == null || pFields.Length == 0)
                {
                    sql.Append(" WHERE ");
                }
                else
                {
                    sql.Append(" AND ");
                }
                sql.Append("(" + pCondition + ")");
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return GetData(cmd);
        }

        public DataSet GetAll()
        {
            return GetData("SELECT * FROM " + TableName);
        }

        public DataSet GetAll(string pTextOrder)
        {
            return GetData("SELECT * FROM " + TableName + " ORDER BY " + pTextOrder);
        }

        public DataSet GetAllById(long pId)
        {
            return GetData("SELECT * FROM " + TableName + " WHERE " + this.ID + "=" + pId);
        }

        public DataSet GetAllByQuery(string pQuery)
        {
            return GetData(pQuery);
        }

        public DataSet GetDataById(long pId, string pSelect)
        {
            return GetData("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId);
        }

        public DataSet GetDataById(long pId, string pSelect, string pCondition)
        {
            if (string.IsNullOrEmpty(pCondition))
            {
                return GetData("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId);
            }
            return GetData("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId + " AND " + pCondition);
        }

        public DataSet GetDataMultiTables(List<string> pTableNames, string pSelect, string pCondition)
        {
            string sqlSelects = "SELECT {0} FROM {1} WHERE {2}";
            for (int i = 0; i < pTableNames.Count; i++)
            {
                pTableNames[i] = string.Format(sqlSelects, pSelect, pTableNames[i], pCondition);
            }
            sqlSelects = string.Join(" ", pTableNames);
            return GetData(sqlSelects);
        }

        public object GetProperty(long pId, string pPropertyName)
        {
            DataSet dsData = GetDataById(pId, pPropertyName);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][pPropertyName];
            }
            return null;
        }

        public object GetMinProperty(string pPropertyName, string pCondition)
        {
            DataSet dsData = GetDataBy("MIN(" + pPropertyName + ")", null, null, pCondition, string.Empty);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][0];
            }
            return null;
        }

        public object GetMaxProperty(string pPropertyName, string pCondition)
        {
            DataSet dsData = GetDataBy("MAX(" + pPropertyName + ")", null, null, pCondition, string.Empty);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][0];
            }
            return null;
        }

        #endregion

        #region "Check Data"

        public bool CheckExistMultiData(string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string sql = "SELECT TOP 1 " + pFields[0] + " FROM " + TableName + " WHERE ";
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sql += " AND ";
                }
                sql += pFields[i] + "=@" + pFields[i];
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = sql;
            DataSet ds = GetData(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        public bool CheckExistMultiData(long pNotId, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string StrNpgsql = "SELECT TOP 1 " + pFields[0] + " FROM " + TableName + " WHERE " + this.ID + "<>" + pNotId;
            for (int i = 0; i < pFields.Length; i++)
            {
                StrNpgsql += " AND " + pFields[i] + "=@" + pFields[i];
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = StrNpgsql;
            DataSet ds = GetData(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        #endregion

        #region "Insert Data"

        public long Insert(string[] pFields, object[] pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();
            //NpgsqlParameter Identity = new NpgsqlParameter("@Identity", 0);
            //Identity.Direction = ParameterDirection.Output;
            //cmd.Parameters.Add(Identity);
            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return ExecuteNonQuery(cmd);
            //if (result > 0 && Identity.Value != null && Identity.Value != DBNull.Value)
            //{
            //    return Convert.ToInt64(Identity.Value);
            //}
        }

        public long Insert(List<string> pFields, List<object> pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();
            //NpgsqlParameter Identity = new NpgsqlParameter("@Identity", 0);
            //Identity.Direction = ParameterDirection.Output;
            //cmd.Parameters.Add(Identity);
            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return ExecuteNonQuery(cmd);

        }

        public long InsertNonIdentity(string[] pFields, object[] pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return ExecuteNonQuery(cmd);
        }

        #endregion

        #region "Update Data"

        public int Update(long pId, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int Update(long pId, List<string> pFields, List<object> pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int Update(long pId, string[] pFields, object[] pDatas, string pCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate = sqlUpdate + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int Update(long pId, List<string> pFields, List<object> pDatas, string pCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate = sqlUpdate + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int Update(string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + pCondition;
            NpgsqlCommand cmd = new NpgsqlCommand();
            List<string> lstSet = new List<string>();
            for (int i = 0; i < pFields.Length; i++)
            {
                lstSet.Add(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, string.Join(",", lstSet.ToArray()));
            return ExecuteNonQuery(cmd);
        }

        public int Update(string[] pFields, object[] pDatas, string[] pFieldCondition, object[] pDataCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0}";
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            if (pFieldCondition != null)
            {
                sqlUpdate += " WHERE ";
                for (int i = 0; i < pFieldCondition.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlUpdate += " AND ";
                    }
                    sqlUpdate += pFieldCondition[i] + "=@" + pFieldCondition[i];
                    cmd.Parameters.Add(new NpgsqlParameter(pFieldCondition[i], pDataCondition[i]));
                }
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int UpdateMulti(long[] pIds, string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate += " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int UpdateMulti(List<long> pIds, string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate += " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return ExecuteNonQuery(cmd);
        }

        public int UpdatePropertyViews(long pId, string pField)
        {
            string sql = "UPDATE {0} SET {1}=ISNULL({1},0)+1 WHERE " + this.ID + "={2}";
            return ExecuteNonQuery(string.Format(sql, this.TableName, pField, pId));
        }

        public int UpdatePropertyViews(string pCondition, string pField)
        {
            string sql = "UPDATE {0} SET {1}=ISNULL({1},0)+1 WHERE {2}";
            return ExecuteNonQuery(string.Format(sql, this.TableName, pField, pCondition));
        }

        #endregion

        #region "Delete Data"

        public int Delete(long pId)
        {
            string strDelete = "DELETE " + TableName + " WHERE " + this.ID + "=" + pId;
            return ExecuteNonQuery(strDelete);
        }

        public int Delete(long pId, string pCondition)
        {
            string sqlCondition = string.Empty;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlCondition = " AND " + pCondition;
            }
            string sqlDelete = "DELETE " + TableName + " WHERE " + this.ID + "=" + pId + sqlCondition;
            return ExecuteNonQuery(sqlDelete);
        }

        public int DeleteMulti(string pCondition, string pFieldIn, long[] pValues, bool pIsNotIn)
        {
            string StrDelete = "DELETE " + TableName + " WHERE " + pFieldIn + " " + (pIsNotIn ? "NOT IN" : "IN") + " (" + string.Join(',', pValues) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                StrDelete += " AND " + pCondition;
            }
            return ExecuteNonQuery(StrDelete);
        }

        public int DeleteMulti(string pCondition, string pFieldIn, List<long> pValues, bool pIsNotIn)
        {
            string StrDelete = "DELETE " + TableName + " WHERE " + pFieldIn + " " + (pIsNotIn ? "NOT IN" : "IN") + " (" + string.Join(',', pValues) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                StrDelete += " AND " + pCondition;
            }
            return ExecuteNonQuery(StrDelete);
        }

        public int DeleteMulti(long[] pIds)
        {
            return DeleteMulti(null, this.ID, pIds, false);
        }

        public int DeleteMulti(List<long> pIds)
        {
            return DeleteMulti(null, this.ID, pIds, false);
        }

        public int DeleteMulti(long[] pIds, string pCondition)
        {
            return DeleteMulti(pCondition, this.ID, pIds, false);
        }

        public int DeleteMulti(List<long> pIds, string pCondition)
        {
            return DeleteMulti(pCondition, this.ID, pIds, false);
        }

        public int DeleteMultiTables(List<string> pTableNames, string pCondition)
        {
            string sqlDelete = "DELETE {0} WHERE {1}";
            for (int i = 0; i < pTableNames.Count; i++)
            {
                pTableNames[i] = string.Format(sqlDelete, pTableNames[i], pCondition);
            }
            sqlDelete = string.Join(" ", pTableNames);
            return ExecuteNonQuery(sqlDelete);
        }

        #endregion

        #region "Search Data"

        public DataSet SearchData(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, List<string> pTextSearchFields, List<object> pTextSearchDatas, List<string> pFieldCondition, List<object> pDataCondition)
        {
            pFrom = pFrom > 0 ? pFrom : 1;
            pTo = pTo > 0 ? pTo : 10;

            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbCondition = new StringBuilder();
            if (pTextSearchFields != null)
            {
                for (int i = 0; i < pTextSearchFields.Count; i++)
                {
                    if (i < pTextSearchDatas.Count)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[i]));
                    }
                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[0]));
                    }
                    if (i > 0)
                    {
                        sbCondition.Append(" OR ");
                    }
                    sbCondition.Append(pTextSearchFields[i] + $" LIKE N'%'+@{pTextSearchFields[i]}+'%'");
                }
            }
            if (pFieldCondition != null)
            {
                for (int i = 0; i < pFieldCondition.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFieldCondition[i], pDataCondition[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (sbCondition.Length > 0)
                {
                    sbCondition.Insert(0, $"({pCondition}) AND (");
                    sbCondition.Append(")");
                }
                else
                {
                    sbCondition.Append(pCondition);
                }
            }
            if (sbCondition.Length > 0)
            {
                sbCondition.Insert(0, " WHERE ");
            }
            pCondition = sbCondition.ToString();
            cmd.CommandText = $@"
SELECT {pSelect}, COUNT(1) OVER() as TotalRow, ROW_NUMBER() OVER(ORDER BY {pTextOrder}) as RowIndex
FROM {this.TableName}{pCondition}
ORDER BY {pTextOrder}
OFFSET {pFrom - 1} ROWS
FETCH FIRST {pTo - pFrom + 1} ROWS ONLY";
            return GetData(cmd);
        }

        public DataSet SearchData(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, List<string> pTextSearchFields, List<object> pTextSearchDatas, List<string> pFieldCondition, List<object> pDataCondition, List<string> pFieldSums)
        {
            pFrom = pFrom > 0 ? pFrom : 1;
            pTo = pTo > 0 ? pTo : 10;

            string sql;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbCondition = new StringBuilder();

            if (pTextSearchFields != null)
            {
                for (int i = 0; i < pTextSearchFields.Count; i++)
                {
                    if (i < pTextSearchDatas.Count)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[i]));
                    }
                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[0]));
                    }
                    if (i > 0)
                    {
                        sbCondition.Append(" OR ");
                    }
                    sbCondition.Append(pTextSearchFields[i] + $" LIKE N'%'+@{pTextSearchFields[i]}+'%'");
                }
            }
            if (pFieldCondition != null)
            {
                for (int i = 0; i < pFieldCondition.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFieldCondition[i], pDataCondition[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (sbCondition.Length > 0)
                {
                    sbCondition.Insert(0, $"({pCondition}) AND (");
                    sbCondition.Append(")");
                }
                else
                {
                    sbCondition.Append(pCondition);
                }
            }
            if (sbCondition.Length > 0)
            {
                sbCondition.Insert(0, " WHERE ");
            }
            pCondition = sbCondition.ToString();
            sql = $@"
SELECT {pSelect}, COUNT(1) OVER() as TotalRow , ROW_NUMBER() OVER(ORDER BY {pTextOrder}) RowIndex
FROM {this.TableName}{pCondition}
ORDER BY {pTextOrder}
OFFSET {pFrom - 1} ROWS
FETCH FIRST {pTo - pFrom + 1} ROWS ONLY";
            if (pFieldSums != null && pFieldSums.Count > 0)
            {
                string sum = string.Join(",", pFieldSums.Select(m => $"SUM({m}) AS {m}").ToArray());
                sql = $"{sql} SELECT {sum} FROM {this.TableName}{pCondition}";
            }
            cmd.CommandText = sql;
            return GetData(cmd);
        }

        #endregion

        #region "Store Procedures"

        public int ExecuteProcedures(string pProcedureName, List<string> pFields, List<object> pDatas)
        {
            int result = -1;
            NpgsqlParameter sqlParam;
            NpgsqlCommand cmd = new NpgsqlCommand(pProcedureName)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (pFields != null)
            {
                for (int i = 0; i < pFields.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                }
            }

            // Get return value in Store Procedure
            sqlParam = new NpgsqlParameter("@Return", NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParam);

            ExecuteNonQuery(cmd);

            if (sqlParam.Value != DBNull.Value && sqlParam.Value != null)
            {
                result = Convert.ToInt32(sqlParam.Value);
            }

            return result;
        }

        public int ExecuteProcedures(string pProcedureName, List<string> pFields, List<object> pDatas, List<string> pOutputFields, ref List<object> pOutputDatas)
        {
            int result = -1;
            NpgsqlParameter sqlParam;
            NpgsqlCommand cmd = new NpgsqlCommand(pProcedureName)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (pFields != null)
            {
                for (int i = 0; i < pFields.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                }
            }

            if (pOutputFields != null)
            {
                for (int i = 0; i < pOutputFields.Count; i++)
                {
                    sqlParam = new NpgsqlParameter("@" + pOutputFields[i], pOutputDatas[i]);
                    if (pOutputDatas[i].GetType() == typeof(string))
                    {
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        sqlParam.Size = 4000;
                    }
                    else if (pOutputDatas[i].GetType() == typeof(int))
                    {
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Integer;
                    }
                    sqlParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlParam);
                }
            }

            // Get return value in Store Procedure
            sqlParam = new NpgsqlParameter("@Return", NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParam);

            ExecuteNonQuery(cmd);

            if (sqlParam.Value != DBNull.Value && sqlParam.Value != null)
            {
                result = Convert.ToInt32(sqlParam.Value);
            }

            if (pOutputFields != null)
            {
                for (int i = 0; i < pOutputFields.Count; i++)
                {
                    pOutputDatas[i] = cmd.Parameters["@" + pOutputFields[i]].Value;
                }
            }
            return result;
        }

        #endregion

        #region "Insert Or Update"

        public int InsertOrUpdate(List<string> pFields, List<List<object>> pDatas, string[] pFieldConditions, string[] pFieldUpdates)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"MERGE INTO {0} AS Target
                                USING   (
                                            VALUES {1} 
                                        ) AS Source ({2})
                                ON {5}
                                WHEN MATCHED THEN
                                UPDATE SET {4}
                                WHEN NOT MATCHED BY TARGET THEN
                                INSERT ({2}) VALUES ({3});";
            // process values
            List<string> lstValues = new List<string>();//1
            List<string> lstCondition = new List<string>();//5
            List<string> lstUpdates = new List<string>();//4
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            string ValueSet = string.Join(",", pFields.Select(s => $"Source.{s}").ToArray());//3
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            for (int i = 0; i < pFieldConditions.Length; i++)
            {
                lstCondition.Add($"Target.{pFieldConditions[i]}=Source.{pFieldConditions[i]}");
            }
            for (int i = 0; i < pFieldUpdates.Length; i++)
            {
                lstUpdates.Add($"{pFieldUpdates[i]}=Source.{pFieldUpdates[i]}");
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", lstValues), string.Join(",", pFields), ValueSet, string.Join(",", lstUpdates), string.Join(" and ", lstCondition));
            return ExecuteNonQuery(cmd);
        }

        public int InsertMultirow(List<string> pFields, List<List<object>> pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"INSERT INTO {0} ({1}) VALUES {2};";
            List<string> lstValues = new List<string>();//2
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", pFields), string.Join(",", lstValues));
            return ExecuteNonQuery(cmd);
        }

        public int UpdateMultiRow(List<string> pFields, List<List<object>> pDatas, string[] pFieldConditions, string[] pFieldUpdates, bool IsViewContent)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"MERGE INTO {0} AS Target
                                USING   (
                                            VALUES {1} 
                                        ) AS Source ({2})
                                ON {3}
                                WHEN MATCHED THEN
                                UPDATE SET {4};";
            // process values
            List<string> lstValues = new List<string>();//1
            List<string> lstCondition = new List<string>();//3
            List<string> lstUpdates = new List<string>();//4
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            for (int i = 0; i < pFieldConditions.Length; i++)
            {
                lstCondition.Add($"Target.{pFieldConditions[i]}=Source.{pFieldConditions[i]}");
            }
            if (IsViewContent)
            {
                for (int i = 0; i < pFieldUpdates.Length; i++)
                {
                    lstUpdates.Add($"{pFieldUpdates[i]}=ISNULL(Target.{pFieldUpdates[i]},0)+Source.{pFieldUpdates[i]}");
                }
            }
            else
            {
                for (int i = 0; i < pFieldUpdates.Length; i++)
                {
                    lstUpdates.Add($"{pFieldUpdates[i]}=Source.{pFieldUpdates[i]}");
                }
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", lstValues), string.Join(",", pFields), string.Join(" and ", lstCondition), string.Join(",", lstUpdates));
            return ExecuteNonQuery(cmd);
        }

        #endregion

        public int ExecuteNonQuery(string pSQL, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            if (pFields != null && pFields.Length > 0)
            {
                for (int i = 0; i < pFields.Length; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            cmd.CommandText = pSQL;
            return ExecuteNonQuery(cmd);
        }

        #endregion

        #region "Async Methods"

        #region "Get Data"

        public async Task<DataSet> GetDataByInIdsAsync(string pSelect, long[] pIds)
        {
            if (pIds == null || pIds.Length == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByInIdsAsync(string pSelect, List<long> pIds)
        {
            if (pIds == null || pIds.Count == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByInIdsAsync(string pSelect, long[] pIds, string pCondition)
        {
            if (pIds == null || pIds.Length == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql = sql + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByInIdsAsync(string pSelect, List<long> pIds, string pCondition)
        {
            if (pIds == null || pIds.Count == 0)
            {
                return null;
            }
            string sql = "SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql = sql + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataAsync(string pSQL, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
            if (pFields != null)
            {
                for (int i = 0; i < pFields.Length; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByAsync(string pSelect, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            cmd.CommandText = sql.ToString();
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSelect"></param>
        /// <param name="pCondition">Phai chua cac doi so cua pFields va pDatas</param>
        /// <param name="pFields"></param>
        /// <param name="pDatas"></param>
        /// <param name="pTextOrder"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataByAsync(string pSelect, string pCondition, string[] pFields, object[] pDatas, string pTextOrder)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                for (int i = 0; i < pFields.Length; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                sql.Append(" WHERE " + pCondition);
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByAsync(string pSelect, string[] pFields, object[] pDatas, string pTextOrder)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSelect"></param>
        /// <param name="pFields"></param>
        /// <param name="pDatas"></param>
        /// <param name="pCondition">Khong chua cac doi so cua pFields va pDatas</param>
        /// <param name="pTextOrder"></param>
        /// <returns></returns>
        public async Task<DataSet> GetDataByAsync(string pSelect, string[] pFields, object[] pDatas, string pCondition, string pTextOrder)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null && pFields.Length > 0)
            {
                sql.Append(" WHERE ");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append(pFields[i] + "=@" + pFields[i]);
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (pFields == null || pFields.Length == 0)
                {
                    sql.Append(" WHERE ");
                }
                else
                {
                    sql.Append(" AND ");
                }
                sql.Append("(" + pCondition + ")");
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByAsync(string pSelect, string[] pFields, object[] pDatas, string pCondition, string pTextOrder, bool pIsSearch)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sql = new StringBuilder("SELECT " + pSelect + " FROM " + TableName);
            if (pFields != null && pFields.Length > 0)
            {
                sql.Append(" WHERE (");
                for (int i = 0; i < pFields.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(pIsSearch ? " OR " : " AND ");
                    }
                    if (pIsSearch)
                    {
                        sql.Append(pFields[i] + " LIKE N'%'+@" + pFields[i] + "+'%'");
                    }
                    else
                    {
                        sql.Append(pFields[i] + "=@" + pFields[i]);
                    }
                    cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
                }
                sql.Append(")");
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (pFields == null || pFields.Length == 0)
                {
                    sql.Append(" WHERE ");
                }
                else
                {
                    sql.Append(" AND ");
                }
                sql.Append("(" + pCondition + ")");
            }
            if (!string.IsNullOrEmpty(pTextOrder))
            {
                sql.Append(" ORDER BY " + pTextOrder);
            }
            cmd.CommandText = sql.ToString();
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> GetAllAsync()
        {
            return await GetDataAsync("SELECT * FROM " + TableName).ConfigureAwait(false);
        }

        public async Task<DataSet> GetAllAsync(string pTextOrder)
        {
            return await GetDataAsync("SELECT * FROM " + TableName + " ORDER BY " + pTextOrder).ConfigureAwait(false);
        }

        public async Task<DataSet> GetAllByIdAsync(long pId)
        {
            return await GetDataAsync("SELECT * FROM " + TableName + " WHERE " + this.ID + "=" + pId).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByIdAsync(long pId, string pSelect)
        {
            return await GetDataAsync("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataByIdAsync(long pId, string pSelect, string pCondition)
        {
            if (string.IsNullOrEmpty(pCondition))
            {
                return GetData("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId);
            }
            return await GetDataAsync("SELECT " + pSelect + " FROM " + TableName + " WHERE " + this.ID + "=" + pId + " AND " + pCondition).ConfigureAwait(false);
        }

        public async Task<DataSet> GetDataMultiTablesAsync(List<string> pTableNames, string pSelect, string pCondition)
        {
            string sqlSelects = "SELECT {0} FROM {1} WHERE {2}";
            for (int i = 0; i < pTableNames.Count; i++)
            {
                pTableNames[i] = string.Format(sqlSelects, pSelect, pTableNames[i], pCondition);
            }
            sqlSelects = string.Join(" ", pTableNames);
            return await GetDataAsync(sqlSelects).ConfigureAwait(false);
        }

        public async Task<object> GetPropertyAsync(long pId, string pPropertyName)
        {
            DataSet dsData = await GetDataByIdAsync(pId, pPropertyName).ConfigureAwait(false);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][pPropertyName];
            }
            return null;
        }

        public async Task<object> GetMinPropertyAsync(string pPropertyName, string pCondition)
        {
            DataSet dsData = await GetDataByAsync("MIN(" + pPropertyName + ")", null, null, pCondition, string.Empty).ConfigureAwait(false);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][0];
            }
            return null;
        }

        public async Task<object> GetMaxPropertyAsync(string pPropertyName, string pCondition)
        {
            DataSet dsData = await GetDataByAsync("MAX(" + pPropertyName + ")", null, null, pCondition, string.Empty).ConfigureAwait(false);
            if (dsData.Tables[0].Rows.Count > 0)
            {
                return dsData.Tables[0].Rows[0][0];
            }
            return null;
        }

        #endregion

        #region "Check Data"

        public async Task<bool> CheckExistMultiDataAsync(string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string sql = "SELECT TOP 1 " + pFields[0] + " FROM " + TableName + " WHERE ";
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sql += " AND ";
                }
                sql += pFields[i] + "=@" + pFields[i];
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = sql;
            DataSet ds = await GetDataAsync(cmd).ConfigureAwait(false);
            return ds.Tables[0].Rows.Count > 0;
        }

        public async Task<bool> CheckExistMultiDataAsync(long pNotId, string[] pFields, object[] pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string StrNpgsql = "SELECT TOP 1 " + pFields[0] + " FROM " + TableName + " WHERE " + this.ID + "<>" + pNotId;
            for (int i = 0; i < pFields.Length; i++)
            {
                StrNpgsql += " AND " + pFields[i] + "=@" + pFields[i];
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = StrNpgsql;
            DataSet ds = await GetDataAsync(cmd).ConfigureAwait(false);
            return ds.Tables[0].Rows.Count > 0;
        }

        #endregion

        #region "Insert Data"

        public async Task<long> InsertAsync(string[] pFields, object[] pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();

            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);

        }

        public async Task<long> InsertAsync(List<string> pFields, List<object> pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();

            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<long> InsertNonIdentityAsync(string[] pFields, object[] pDatas)
        {
            string sqlInsert = "INSERT INTO {0}({1}) VALUES({2})";
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbField = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbField.Append(",");
                    sbValue.Append(",");
                }
                sbField.Append(pFields[i]);
                sbValue.Append("@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlInsert, TableName, sbField, sbValue);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        #endregion

        #region "Update Data"

        public async Task<int> UpdateAsync(long pId, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(long pId, List<string> pFields, List<object> pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(long pId, string[] pFields, object[] pDatas, string pCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate = sqlUpdate + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(long pId, List<string> pFields, List<object> pDatas, string pCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + "=" + pId;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate = sqlUpdate + " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Count; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + pCondition;
            NpgsqlCommand cmd = new NpgsqlCommand();
            List<string> lstSet = new List<string>();
            for (int i = 0; i < pFields.Length; i++)
            {
                lstSet.Add(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, string.Join(",", lstSet.ToArray()));
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(string[] pFields, object[] pDatas, string[] pFieldCondition, object[] pDataCondition)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0}";
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            if (pFieldCondition != null)
            {
                sqlUpdate += " WHERE ";
                for (int i = 0; i < pFieldCondition.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlUpdate += " AND ";
                    }
                    sqlUpdate += pFieldCondition[i] + "=@" + pFieldCondition[i];
                    cmd.Parameters.Add(new NpgsqlParameter(pFieldCondition[i], pDataCondition[i]));
                }
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateMultiAsync(long[] pIds, string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate += " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateMultiAsync(List<long> pIds, string pCondition, string[] pFields, object[] pDatas)
        {
            string sqlUpdate = "UPDATE " + TableName + " SET {0} WHERE " + this.ID + " IN (" + string.Join(',', pIds) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlUpdate += " AND " + pCondition;
            }
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbSet = new StringBuilder();
            for (int i = 0; i < pFields.Length; i++)
            {
                if (i > 0)
                {
                    sbSet.Append(",");
                }
                sbSet.Append(pFields[i] + "=@" + pFields[i]);
                cmd.Parameters.Add(new NpgsqlParameter(pFields[i], pDatas[i]));
            }
            cmd.CommandText = string.Format(sqlUpdate, sbSet);
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdatePropertyViewsAsync(long pId, string pField)
        {
            string sql = "UPDATE {0} SET {1}=ISNULL({1},0)+1 WHERE " + this.ID + "={2}";
            return await ExecuteNonQueryAsync(string.Format(sql, this.TableName, pField, pId)).ConfigureAwait(false);
        }

        public async Task<int> UpdatePropertyViewsAsync(string pCondition, string pField)
        {
            string sql = "UPDATE {0} SET {1}=ISNULL({1},0)+1 WHERE {2}";
            return await ExecuteNonQueryAsync(string.Format(sql, this.TableName, pField, pCondition)).ConfigureAwait(false);
        }

        #endregion

        #region "Delete Data"

        public async Task<int> DeleteAsync(long pId)
        {
            string strDelete = "DELETE " + TableName + " WHERE " + this.ID + "=" + pId;
            return await ExecuteNonQueryAsync(strDelete).ConfigureAwait(false);
        }

        public async Task<int> DeleteAsync(long pId, string pCondition)
        {
            string sqlCondition = string.Empty;
            if (!string.IsNullOrEmpty(pCondition))
            {
                sqlCondition = " AND " + pCondition;
            }
            string sqlDelete = "DELETE " + TableName + " WHERE " + this.ID + "=" + pId + sqlCondition;
            return await ExecuteNonQueryAsync(sqlDelete).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(string pCondition, string pFieldIn, long[] pValues, bool pIsNotIn)
        {
            string StrDelete = "DELETE " + TableName + " WHERE " + pFieldIn + " " + (pIsNotIn ? "NOT IN" : "IN") + " (" + string.Join(',', pValues) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                StrDelete += " AND " + pCondition;
            }
            return await ExecuteNonQueryAsync(StrDelete).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(string pCondition, string pFieldIn, List<long> pValues, bool pIsNotIn)
        {
            string StrDelete = "DELETE " + TableName + " WHERE " + pFieldIn + " " + (pIsNotIn ? "NOT IN" : "IN") + " (" + string.Join(',', pValues) + ")";
            if (!string.IsNullOrEmpty(pCondition))
            {
                StrDelete += " AND " + pCondition;
            }
            return await ExecuteNonQueryAsync(StrDelete).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(long[] pIds)
        {
            return await DeleteMultiAsync(null, this.ID, pIds, false).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(List<long> pIds)
        {
            return await DeleteMultiAsync(null, this.ID, pIds, false).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(long[] pIds, string pCondition)
        {
            return await DeleteMultiAsync(pCondition, this.ID, pIds, false).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiAsync(List<long> pIds, string pCondition)
        {
            return await DeleteMultiAsync(pCondition, this.ID, pIds, false).ConfigureAwait(false);
        }

        public async Task<int> DeleteMultiTablesAsync(List<string> pTableNames, string pCondition)
        {
            string sqlDelete = "DELETE {0} WHERE {1}";
            for (int i = 0; i < pTableNames.Count; i++)
            {
                pTableNames[i] = string.Format(sqlDelete, pTableNames[i], pCondition);
            }
            sqlDelete = string.Join(" ", pTableNames);
            return await ExecuteNonQueryAsync(sqlDelete).ConfigureAwait(false);
        }

        #endregion

        #region "Search Data"

        public async Task<DataSet> SearchAsync(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, string[] pTextSearchFields, object[] pTextSearchDatas, string[] pFieldCondition, object[] pDataCondition)
        {
            return await SearchAsync(pFrom, pTo, pTextOrder, pSelect, pCondition, pTextSearchFields, pTextSearchDatas, pFieldCondition, pDataCondition, null).ConfigureAwait(false);
        }

        public async Task<DataSet> SearchAsync(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, List<string> pTextSearchFields, List<object> pTextSearchDatas, List<string> pFieldCondition, List<object> pDataCondition)
        {
            return await SearchAsync(pFrom, pTo, pTextOrder, pSelect, pCondition, pTextSearchFields, pTextSearchDatas, pFieldCondition, pDataCondition, null).ConfigureAwait(false);
        }

        public async Task<DataSet> SearchAsync(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, string[] pTextSearchFields, object[] pTextSearchDatas, string[] pFieldCondition, object[] pDataCondition, string[] pFieldSums)
        {
            return await SearchAsync(pFrom, pTo, pTextOrder, pSelect, pCondition, pTextSearchFields?.ToList(), pTextSearchDatas?.ToList(), pFieldCondition?.ToList(), pDataCondition?.ToList(), pFieldSums?.ToList()).ConfigureAwait(false);
        }

        public async Task<DataSet> SearchAsync(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, List<string> pTextSearchFields, List<object> pTextSearchDatas, List<string> pFieldCondition, List<object> pDataCondition, List<string> pFieldSums)
        {
            string sql = "WITH ViewSplitPages AS ( SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RowIndex, " + pSelect + " FROM " + this.TableName + " {1} ) , GetTotalRow AS (SELECT MAX(RowIndex) AS TotalRow FROM ViewSplitPages) SELECT * FROM ViewSplitPages, GetTotalRow WHERE RowIndex BETWEEN " + pFrom + " AND " + pTo;
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbCondition = new StringBuilder();
            if (pTextSearchFields != null)
            {
                for (int i = 0; i < pTextSearchFields.Count; i++)
                {
                    if (i < pTextSearchDatas.Count)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@" + pTextSearchFields[i], pTextSearchDatas[i]));
                    }
                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@" + pTextSearchFields[i], pTextSearchDatas[0]));
                    }
                    if (i > 0)
                    {
                        sbCondition.Append(" OR ");
                    }
                    sbCondition.Append(pTextSearchFields[i] + " LIKE N'%'+@" + pTextSearchFields[i] + "+'%'");
                }
            }
            if (pFieldCondition != null)
            {
                for (int i = 0; i < pFieldCondition.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFieldCondition[i], pDataCondition[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (sbCondition.Length > 0)
                {
                    sbCondition.Insert(0, "(" + pCondition + ") AND (");
                    sbCondition.Append(")");
                }
                else
                {
                    sbCondition.Append(pCondition);
                }
            }
            if (sbCondition.Length > 0)
            {
                sbCondition.Insert(0, " WHERE ");
            }
            pCondition = sbCondition.ToString();
            if (pFieldSums != null && pFieldSums.Count > 0)
            {
                string sum = string.Join(",", pFieldSums.Select(m => $"SUM({m}) AS {m}").ToArray());
                sql = $"{sql} SELECT {sum} FROM {this.TableName} {pCondition}";
            }
            cmd.CommandText = string.Format(sql, pTextOrder, pCondition);
            return await GetDataAsync(cmd).ConfigureAwait(false);
        }

        public async Task<DataSet> SearchDataAsync(int pFrom, int pTo, string pTextOrder, string pSelect, string pCondition, List<string> pTextSearchFields, List<object> pTextSearchDatas, List<string> pFieldCondition, List<object> pDataCondition)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            StringBuilder sbCondition = new StringBuilder();
            if (pTextSearchFields != null)
            {
                for (int i = 0; i < pTextSearchFields.Count; i++)
                {
                    if (i < pTextSearchDatas.Count)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[i]));
                    }
                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter($"@{pTextSearchFields[i]}", pTextSearchDatas[0]));
                    }
                    if (i > 0)
                    {
                        sbCondition.Append(" OR ");
                    }
                    sbCondition.Append(pTextSearchFields[i] + $" LIKE N'%'+@{pTextSearchFields[i]}+'%'");
                }
            }
            if (pFieldCondition != null)
            {
                for (int i = 0; i < pFieldCondition.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFieldCondition[i], pDataCondition[i]));
                }
            }
            if (!string.IsNullOrEmpty(pCondition))
            {
                if (sbCondition.Length > 0)
                {
                    sbCondition.Insert(0, $"({pCondition}) AND (");
                    sbCondition.Append(")");
                }
                else
                {
                    sbCondition.Append(pCondition);
                }
            }
            if (sbCondition.Length > 0)
            {
                sbCondition.Insert(0, " WHERE ");
            }
            pCondition = sbCondition.ToString();

            cmd.CommandText = $@"
SELECT COUNT({this.ID}) AS TotalRow FROM {this.TableName}{pCondition}
SELECT * 
FROM (SELECT ROW_NUMBER() OVER(ORDER BY {pTextOrder}) AS RowIndex, {pSelect} FROM {this.TableName}{pCondition}) temp 
WHERE RowIndex BETWEEN {pFrom} AND {pTo}";
            DataSet dsData = await GetDataAsync(cmd).ConfigureAwait(false);
            dsData.Tables[1].Columns.Add("TotalRow", typeof(int));
            if (dsData.Tables[1].Rows.Count > 0)
            {
                int totalRow = Convert.ToInt32(dsData.Tables[0].Rows[0][0]);
                for (int index = 0; index < dsData.Tables[1].Rows.Count; index++)
                {
                    dsData.Tables[1].Rows[index]["TotalRow"] = totalRow;
                }
            }
            dsData.Tables.RemoveAt(0);
            return dsData;
        }

        #endregion

        #region "Store Procedures"

        public async Task<int> ExecuteProceduresAsync(string pProcedureName, List<string> pFields, List<object> pDatas)
        {
            int result = -1;
            NpgsqlParameter sqlParam;
            NpgsqlCommand cmd = new NpgsqlCommand(pProcedureName)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (pFields != null)
            {
                for (int i = 0; i < pFields.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                }
            }

            // Get return value in Store Procedure
            sqlParam = new NpgsqlParameter("@Return", NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParam);

            await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);

            if (sqlParam.Value != DBNull.Value && sqlParam.Value != null)
            {
                result = Convert.ToInt32(sqlParam.Value);
            }

            return result;
        }

        public async Task<List<object>> ExecuteProceduresAsync(string pProcedureName, List<string> pFields, List<object> pDatas, List<string> pOutputFields, List<object> pOutputDatas)
        {
            int result = -1;
            NpgsqlParameter sqlParam;
            NpgsqlCommand cmd = new NpgsqlCommand(pProcedureName)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (pFields != null)
            {
                for (int i = 0; i < pFields.Count; i++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                }
            }

            if (pOutputFields != null)
            {
                for (int i = 0; i < pOutputFields.Count; i++)
                {
                    sqlParam = new NpgsqlParameter("@" + pOutputFields[i], pOutputDatas[i]);
                    if (pOutputDatas[i].GetType() == typeof(string))
                    {
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        sqlParam.Size = 4000;
                    }
                    else if (pOutputDatas[i].GetType() == typeof(int))
                    {
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Integer;
                    }
                    sqlParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlParam);
                }
            }

            // Get return value in Store Procedure
            sqlParam = new NpgsqlParameter("@Return", NpgsqlDbType.Integer)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParam);

            await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);

            if (sqlParam.Value != DBNull.Value && sqlParam.Value != null)
            {
                result = Convert.ToInt32(sqlParam.Value);
            }

            if (pOutputFields != null)
            {
                for (int i = 0; i < pOutputFields.Count; i++)
                {
                    pOutputDatas[i] = cmd.Parameters["@" + pOutputFields[i]].Value;
                }
            }
            return pOutputDatas;
        }

        #endregion

        #region "Insert Or Update"

        public async Task<int> InsertOrUpdateAsync(List<string> pFields, List<List<object>> pDatas, string[] pFieldConditions, string[] pFieldUpdates)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"MERGE INTO {0} AS Target
                                USING   (
                                            VALUES {1} 
                                        ) AS Source ({2})
                                ON {5}
                                WHEN MATCHED THEN
                                UPDATE SET {4}
                                WHEN NOT MATCHED BY TARGET THEN
                                INSERT ({2}) VALUES ({3});";
            // process values
            List<string> lstValues = new List<string>();//1
            List<string> lstCondition = new List<string>();//5
            List<string> lstUpdates = new List<string>();//4
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            string ValueSet = string.Join(",", pFields.Select(s => $"Source.{s}").ToArray());//3
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            for (int i = 0; i < pFieldConditions.Length; i++)
            {
                lstCondition.Add($"Target.{pFieldConditions[i]}=Source.{pFieldConditions[i]}");
            }
            for (int i = 0; i < pFieldUpdates.Length; i++)
            {
                lstUpdates.Add($"{pFieldUpdates[i]}=Source.{pFieldUpdates[i]}");
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", lstValues), string.Join(",", pFields), ValueSet, string.Join(",", lstUpdates), string.Join(" and ", lstCondition));
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> InsertMultirowAsync(List<string> pFields, List<List<object>> pDatas)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"INSERT INTO {0} ({1}) VALUES {2};";
            List<string> lstValues = new List<string>();//2
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", pFields), string.Join(",", lstValues));
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        public async Task<int> UpdateMultiRowAsync(List<string> pFields, List<List<object>> pDatas, string[] pFieldConditions, string[] pFieldUpdates, bool IsViewContent)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            string strsql = @"MERGE INTO {0} AS Target
                                USING   (
                                            VALUES {1} 
                                        ) AS Source ({2})
                                ON {3}
                                WHEN MATCHED THEN
                                UPDATE SET {4};";
            // process values
            List<string> lstValues = new List<string>();//1
            List<string> lstCondition = new List<string>();//3
            List<string> lstUpdates = new List<string>();//4
            string ValueItem = $"({string.Join(",", pFields.Select(s => $"@{s}_{{0}}").ToArray())})";
            for (int i = 0; i < pDatas.Count; i++)
            {
                for (int j = 0; j < pFields.Count; j++)
                {
                    cmd.Parameters.Add(new NpgsqlParameter($"@{pFields[j]}_{i}", pDatas[i][j]));
                }
                lstValues.Add(string.Format(ValueItem, i));
            }
            for (int i = 0; i < pFieldConditions.Length; i++)
            {
                lstCondition.Add($"Target.{pFieldConditions[i]}=Source.{pFieldConditions[i]}");
            }
            if (IsViewContent)
            {
                for (int i = 0; i < pFieldUpdates.Length; i++)
                {
                    lstUpdates.Add($"{pFieldUpdates[i]}=ISNULL(Target.{pFieldUpdates[i]},0)+Source.{pFieldUpdates[i]}");
                }
            }
            else
            {
                for (int i = 0; i < pFieldUpdates.Length; i++)
                {
                    lstUpdates.Add($"{pFieldUpdates[i]}=Source.{pFieldUpdates[i]}");
                }
            }
            cmd.CommandText = string.Format(strsql, TableName, string.Join(",", lstValues), string.Join(",", pFields), string.Join(" and ", lstCondition), string.Join(",", lstUpdates));
            return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
        }

        #endregion

        #endregion
    }
}