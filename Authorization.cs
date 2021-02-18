using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPS.DataBase
{
    public partial class Authorization : Form
    {
        public bool IsLogin
        {
            get
            {
                bool been = false;
                string loginUser = textBoxLogin.Text;
                string passwordUser = textBoxPassword.Text;

                DatabaseManager _databaseManager = new DatabaseManager();
                DataTable _dataTable = new DataTable();
                MySqlDataAdapter _mySqlDataAdapter = new MySqlDataAdapter();
                MySqlCommand _mySqlCommand = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @UserLogin", _databaseManager.GetConnection);//выбираем все записи из таблички user где логин = введеному логину и пароль = введеному паролю

                //меняем заглушки на переменные
                _mySqlCommand.Parameters.Add("@UserLogin", MySqlDbType.VarChar).Value = loginUser;

                _mySqlDataAdapter.SelectCommand = _mySqlCommand;//выбираем команду
                _mySqlDataAdapter.Fill(_dataTable);//заполянем данные в табличку ***

                if (_dataTable.Rows.Count > 0)
                {
                    been = true;
                }
                else
                    been = false;

                return been;
            }
        }

        public Authorization()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, EventArgs e)
        {
            string loginUser = textBoxLogin.Text;
            string passwordUser = textBoxPassword.Text;

            DatabaseManager _databaseManager = new DatabaseManager();
            DataTable _dataTable = new DataTable();
            MySqlDataAdapter _mySqlDataAdapter = new MySqlDataAdapter();
            MySqlCommand _mySqlCommand = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @UserLogin AND `password` = @UserPassword", _databaseManager.GetConnection);//выбираем все записи из таблички user где логин = введеному логину и пароль = введеному паролю

            try
            {
                //меняем заглушки на переменные
                _mySqlCommand.Parameters.Add("@UserLogin", MySqlDbType.VarChar).Value = loginUser;
                _mySqlCommand.Parameters.Add("@UserPassword", MySqlDbType.VarChar).Value = passwordUser;
                
                _databaseManager.OpenConnection();//открываем соеденения 

                _mySqlDataAdapter.SelectCommand = _mySqlCommand;//выбираем команду
                _mySqlDataAdapter.Fill(_dataTable);//заполянем данные в табличку

                if (_dataTable.Rows.Count > 0)
                {
                    //елси уже зарегестрирован
                    DataForm form = new DataForm();
                    this.Hide();
                    form.Show();

                    //запомним кто вошел 
                    User user = new User(loginUser);
                }
                else
                {
                    //проверим, если такого логина нет
                    if (IsLogin)
                    {
                        MessageBox.Show("Пароль введен не верно!", "Внимание!");
                    }
                    else
                    {
                        if (MessageBox.Show("Вы у нас впервые!\nНеобходимо зарегистрироваться!\nЗарегистрироваться сейчас?",
                            "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Registration form = new Registration();
                            this.Hide();
                            form.Show();
                        }
                    }
                       
                }
            }
            catch
            {
                MessageBox.Show("Ошибка работы с базой данных!", "Ошибка");
            }
            finally
            {
                _databaseManager.CloseConnection();//закрываем соеденение
            }
               
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Registration form = new Registration();
            this.Hide();
            form.Show();
        }

    }
}
