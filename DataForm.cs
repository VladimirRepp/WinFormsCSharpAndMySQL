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
    public partial class DataForm : Form
    {
        //поле пользователь
        private User user;
        public DataForm()
        {
            InitializeComponent();
        }
        private void обПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа демонстриует возможности работ Windows Forms, C# и MySql." +
                "\nТак же, пример создания базы данных на основе этих инструментов." +
                "\nДанные окно показывает возможность отобржения данных загруженных с сервера.", "Информация об программе!");
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void выходИзПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void вернутьсяКОкнуВходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authorization form = new Authorization();
            this.Hide();
            form.Show();
        }
        private void вернутьсяКОкнуРегистрацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registration form = new Registration();
            this.Hide();
            form.Show();
        }

       //формирование шапки таблицы
        private void HeaderOfTheTable()
        {
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Номер"; //текст в шапке
            column1.Width = 100; //ширина колонки
            column1.Name = "id"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Спецификация";
            column2.Width = 100;
            column2.Name = "secification";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Инофрмация";
            column3.Width = 400;
            column3.Name = "information";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Сроки";
            column4.Width = 120;
            column4.Name = "time_constraints";
            column4.CellTemplate = new DataGridViewTextBoxCell();

            dataGrid.Columns.Add(column1);
            dataGrid.Columns.Add(column2);
            dataGrid.Columns.Add(column3);
            dataGrid.Columns.Add(column4);

            dataGrid.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGrid.ReadOnly = true;//запрещаем пользователю изменять данные (!!!)
        }

        //добавление данных в табицу
        private void AddDataGrid(RowOfData row)
        {
            dataGrid.Rows.Add(row.id, row.secification, row.information, row.time_constraints);//добавляем строку в таблицу
        }
        private void DataForm_Shown(object sender, EventArgs e)
        {
            //создаем объект класса пользователь
            user = new User();

            HeaderOfTheTable();//создаем шапку таблицы
            List<RowOfData> _data = new List<RowOfData>();//создаем список данных

            //открываем базу данных и считываем с нее данные
            DatabaseManager _manager = new DatabaseManager();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM `projects`", _manager.GetConnection);
            MySqlDataReader _reader;     

            try
            {
                _manager.OpenConnection();
                _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    //заполняем данные
                    RowOfData row = new RowOfData(_reader["id"],_reader["specification"], _reader["information"], _reader["time_constraints"]);
                    _data.Add(row);
                }

                //добавляем в таблицу данные
                for (int i = 0; i < _data.Count; i++)
                    AddDataGrid(_data[i]);

            }
            catch
            {
                MessageBox.Show("Ошибка работы с базой данных!", "Ошибка!");
            }
            finally
            {
                _manager.CloseConnection();
            }
        }
        private void редакцияДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.userName == "admin" || user.userName == "editor")
            {
                if (MessageBox.Show("Перейти в окно редактирования данных?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataUploadForm form = new DataUploadForm();
                    this.Hide();
                    form.Show();
                }
            }
            else
                MessageBox.Show("У вас нет доступа, чтобы совершить это действие!", "Ошибка!");
        }
        private void обновитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RowOfData> _data = new List<RowOfData>();//создаем список данных

            //открываем базу данных и считываем с нее данные
            DatabaseManager _manager = new DatabaseManager();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM `projects`", _manager.GetConnection);
            MySqlDataReader _reader;

            _manager.OpenConnection();
            _reader = _command.ExecuteReader();

            //удаляем все текущие строки, чтобы обновить данные
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();

            try
            {
                while (_reader.Read())
                {
                    //заполняем данные
                    RowOfData row = new RowOfData(_reader["id"], _reader["specification"], _reader["information"], _reader["time_constraints"]);
                    _data.Add(row);
                }

                //добавляем в таблицу данные
                for (int i = 0; i < _data.Count; i++)
                    AddDataGrid(_data[i]);

                MessageBox.Show("Данные обновлены!", "Внимание!");

            }
            catch
            {
                MessageBox.Show("Ошибка работы с базой данных!", "Ошибка!");
            }
            finally
            {
                _manager.CloseConnection();
            }
        }

        private void дейстияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
