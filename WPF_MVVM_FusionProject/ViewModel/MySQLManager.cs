using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using WPF_MVVM_FusionProject.ViewModel;

namespace WPF_MVVM_FusionProject.DBConncet
{
    public class MySQLManager
    {
        private MySqlConnection connection;

        // Database Initialize
        public void Initialize()
        {
            Debug.WriteLine("Database Initialize");

            string connectionPath = $"SERVER={Config.Server};DATABASE={Config.Database};UID={Config.UserID};PASSWORD={Config.UserPassword}";
            connection = new MySqlConnection(connectionPath);
            OpenMySqlConnection();
        }

        // Create MySqlCommand
        public MySqlCommand CreateCommand(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command;
        }

        // Database Connection
        public bool OpenMySqlConnection()
        {
            try
            {
                connection.Open();
                Debug.WriteLine("데이터베이스 연결 성공");
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Debug.WriteLine("데이터베이스 서버에 연결할 수 없습니다.");
                        break;
                    case 1045:
                        Debug.WriteLine("유저 ID 또는 Password를 확인해주세요.");
                        break;
                }
                return false;
            }
        }

        // Database Close
        public bool CloseMySqlConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // Query Executer(Insert, Delete, Update ...)
        public bool MySqlQueryExecuter(string query)
        {
            MySqlCommand command = CreateCommand(query);

            if (command.ExecuteNonQuery() == 1)
            {
                Debug.WriteLine("값 저장 성공");
                return true;
            }
            else
            {
                Debug.WriteLine("값 저장 실패");
                return false;
            }
        }

        public bool MySqlImageInsertExecuter(string query, byte[] imgByteArr)
        {
            MySqlCommand command = CreateCommand(query);
            command.Parameters.Add(new MySqlParameter("img", imgByteArr));

            if (command.ExecuteNonQuery() == 1)
            {
                Debug.WriteLine("값 저장 성공");
                return true;
            }
            else
            {
                Debug.WriteLine("값 저장 실패");
                return false;
            }
        }

        // Query Executer Select
        public DataSet Select(string query, string tableName)
        {
            DataSet dataSet = new DataSet();

            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                dataAdapter.Fill(dataSet, tableName);
                if (dataSet.Tables.Count <= 0)
                {
                    return null;
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }

            return dataSet;
        }

        //public DataSet SelectAll(string tableName)
        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();

        //        string query = $"SELECT * FROM {tableName}";
        //        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
        //        dataAdapter.Fill(dataSet, tableName);
        //        if (dataSet.Tables.Count > 0)
        //        {
        //            return dataSet;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //}

        //public DataSet SelectDetail(string tableName, string condition, string whereColumn, string whereData)
        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();

        //        string query = $"SELECT {condition} FROM {tableName} WHERE {whereColumn} = '{whereData}'";

        //        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
        //        dataAdapter.Fill(dataSet, tableName);
        //        if (dataSet.Tables.Count > 0)
        //        {
        //            return dataSet;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //}
    }
}
