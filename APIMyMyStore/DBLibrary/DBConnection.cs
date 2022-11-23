using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace DBLibrary
{
    public class DBConnection
    {
        private NpgsqlConnection con = null;
        private NpgsqlTransaction transaction = null;
        private string connectionSQL = string.Empty;

        public DBConnection(string pConnectionSQL)
        {
            connectionSQL = pConnectionSQL;
        }

        #region "Sync Methods"

        private void OpenConnect()
        {
            if (con == null)
            {
                con = new NpgsqlConnection(connectionSQL);
            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }
        
        public void CloseConnection()
        {
            try
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            catch { }
            finally
            {
                transaction = null;
            }
        }

        public void BeginTransaction()
        {
            OpenConnect();
            transaction = con.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
            CloseConnection();
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
            CloseConnection();
        }
        
        public DataSet GetData(String pSQL)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                return GetData(cmd);
            }
            catch
            {
                throw;
            }
        }

        public DataSet GetData(NpgsqlCommand pNpgsqlCommand)
        {
            DataSet dsData = null;
            try
            {
                OpenConnect();
                pNpgsqlCommand.Connection = con;
                if (transaction != null)
                {
                    pNpgsqlCommand.Transaction = transaction;
                }
                dsData = new DataSet();
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(pNpgsqlCommand);
                adap.Fill(dsData);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection();
                }
            }
            return dsData;
        }
        
        public int ExecuteNonQuery(String pSQL)
        {
            int res = -1;
            try
            {
                OpenConnect();
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                cmd.Connection = con;
                if (transaction != null)
                {
                    cmd.Transaction = transaction;
                }
                res = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection();
                }
            }
            return res;
        }

        public int ExecuteNonQuery(NpgsqlCommand pNpgsqlCommand)
        {
            int res = -1;
            try
            {
                OpenConnect();
                pNpgsqlCommand.Connection = con;
                if (transaction != null)
                {
                    pNpgsqlCommand.Transaction = transaction;
                }
                res = pNpgsqlCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    CloseConnection();
                }
            }
            return res;
        }

        public int ExcuteCommand(string pSQL, string[] pFields, object[] pDatas, bool pIsStore)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                if (pIsStore)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (pFields != null)
                {
                    for (int i = 0; i < pFields.Length; i++)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                    }
                }
                return ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region "Async Methods"

        private async Task OpenConnectAsync()
        {
            if (con == null)
            {
                con = new NpgsqlConnection(connectionSQL);
            }
            if (con.State != ConnectionState.Open)
            {
                await con.OpenAsync().ConfigureAwait(false);
            }
        }
        
        public async Task CloseConnectionAsync()
        {
            try
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            catch { }
            finally
            {
                transaction = null;
            }
        }
        
        public async Task BeginTransactionAsync()
        {
            await OpenConnectAsync().ConfigureAwait(false);
            transaction = (NpgsqlTransaction)await con.BeginTransactionAsync().ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            await CloseConnectionAsync().ConfigureAwait(false);
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
            }
            await CloseConnectionAsync().ConfigureAwait(false);
        }
        
        public async Task<DataSet> GetDataAsync(String pSQL)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                return await GetDataAsync(cmd).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataSet> GetDataAsync(NpgsqlCommand pNpgsqlCommand)
        {
            DataSet dsData = null;
            try
            {
                await OpenConnectAsync().ConfigureAwait(false);
                pNpgsqlCommand.Connection = con;
                if (transaction != null)
                {
                    pNpgsqlCommand.Transaction = transaction;
                }

                using (NpgsqlDataReader reader = await pNpgsqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    dsData = new DataSet();
                    while (!reader.IsClosed)
                    {
                        try
                        {
                            DataTable tbl = new DataTable();
                            tbl.Load(reader);
                            dsData.Tables.Add(tbl);
                        }
                        catch
                        {
                            break;
                        }
                    }
                    reader.Close();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    await CloseConnectionAsync().ConfigureAwait(false);
                }
            }
            return dsData;
        }
        
        public async Task<int> ExecuteNonQueryAsync(String pSQL)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(NpgsqlCommand pNpgsqlCommand)
        {
            int res = -1;
            try
            {
                await OpenConnectAsync().ConfigureAwait(false);
                pNpgsqlCommand.Connection = con;
                if (transaction != null)
                {
                    pNpgsqlCommand.Transaction = transaction;
                }
                res = await pNpgsqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    await CloseConnectionAsync().ConfigureAwait(false);
                }
            }
            return res;
        }

        public async Task<int> ExcuteCommandAsync(string pSQL, string[] pFields, object[] pDatas, bool pIsStore)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSQL);
                if (pIsStore)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (pFields != null)
                {
                    for (int i = 0; i < pFields.Length; i++)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@" + pFields[i], pDatas[i]));
                    }
                }
                return await ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}