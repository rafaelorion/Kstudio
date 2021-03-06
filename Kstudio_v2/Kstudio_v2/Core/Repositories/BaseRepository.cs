﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.IO;

namespace Kstudio_v2.Core.Repositories
{
    public abstract class BaseRepository
    {
        private const string DataBaseFile = "banco.db";
        private string ConnectionString;
        private string Database;

        private const string Sql_CreateClientes = "CREATE TABLE Clientes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Banda varchar(255), Responsavel varchar(255), EstiloMusical varchar(255), Email varchar(255), Telefone varchar(255)";
        // private const string Sql_CreateProduto = "CREATE TABLE Produtos (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome varchar(255), Descricao varchar(255),Quantidade INTEGER, Ativo bool)";

        public BaseRepository()
        {
            Database = Path.Combine(System.AppContext.BaseDirectory, "App_Data", DataBaseFile);
            ConnectionString = string.Format("Data Source={0};Version=3;", Database);
            CreateDatabaseIfDoNotExist();
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        public void CreateDatabaseIfDoNotExist()
        {
            var exist = File.Exists(Database);
            if (!exist)
            {
                SQLiteConnection.CreateFile(Database);
                ExecuteCommand(Sql_CreateClientes);
               // ExecuteCommand(Sql_CreateProduto);
            }
        }

        public bool ExecuteCommand(string sqlcommand)
        {
            try
            {
                var connection = GetConnection();
                connection.Open();
                var command = new SQLiteCommand(connection);


                command.CommandText = sqlcommand;
                var result = command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();
                connection.Dispose();

                return result > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}