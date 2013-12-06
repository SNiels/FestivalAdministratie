﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Database
    {
        private const string CONNECTIONSTRING = "DefaultConnection";

        private static ConnectionStringSettings ConnectionString        //reference toevoegen naar System.Configuration, connectionstrings te vinden op www.connectionstrings.com
        {
            get { return ConfigurationManager.ConnectionStrings[CONNECTIONSTRING]; }
        }
        private static DbConnection GetConnection()
        {
            try
            {
                DbConnection con = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
                con.ConnectionString = ConnectionString.ConnectionString;
                con.Open();
                return con;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
                
        }
        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            { 
                con.Close();
                con = null;
            }
        }
        #region Transaction
        public static DbTransaction BeginTransaction()
        {
            DbConnection con = null;
            try
            {
                con = GetConnection();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (con != null)
                    ReleaseConnection(con);
                throw;
            }
        }
        private static DbCommand BuildCommand(DbTransaction trans, string sql, params DbParameter[] parameters)          //params betekent optioneel
        {
            DbCommand command = trans.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sql;
            command.Transaction = trans;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }
        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null)
                    reader.Close();
                if (command != null)
                    ReleaseConnection(command.Connection);
                throw;
            }
        }
        public static int ModifyData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                    ReleaseConnection(command.Connection);
                return 0;
                throw;
            }
        } 
        #endregion
        private static DbCommand BuildCommand(string sql, params DbParameter[] parameters)          //params betekent optioneel
        {
            DbCommand command = GetConnection().CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sql;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }
        public static DbDataReader GetData(string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null)
                    reader.Close();
                if (command != null)
                    ReleaseConnection(command.Connection);
                throw;
            }
        }
        public static int ModifyData(string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(sql, parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                    ReleaseConnection(command.Connection);
                return 0;
                throw;
            }
        } 
        public DbParameter AddParameter(string name, object value)
        {
            DbParameter par = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            par.ParameterName = name;
            par.Value = value;
            return par;
        }
    }
}