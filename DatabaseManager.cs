using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPS.DataBase
{
    class DatabaseManager
    {
        //создаем поле данных соеденения с MySql
        MySqlConnection connection = new MySqlConnection("server=localhost;" +
            "port=3306;" +
            "username=root;" +
            "password=;" +
            "database=fps.database");

        //метод, который открывает соеденение
        public void OpenConnection()
        {
            //проверяем состояние подключения
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        //закрываем соеденение
        public void CloseConnection()
        {
            //проверяем, есть ли соеденение 
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        //возращаем значения объекта соеденения
        public MySqlConnection GetConnection { get { return connection; } }
        
    }
}
