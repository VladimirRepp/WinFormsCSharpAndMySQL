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
    public partial class DataUploadForm : Form
    {
        private List<RowOfData> _data = new List<RowOfData>();//создаем список данных
        private User user;
        public DataUploadForm()
        {
            InitializeComponent();
        }

        private void выходИзПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Вы точно сохранили все изменения с базой данных?" +
                "\nИначе все изменения не сохранятся!","Внимание!",MessageBoxButtons.YesNo) == DialogResult.Yes) 
                Application.Exit();
        }

        private void вернутьсяКОкнуВходаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно сохранили все изменения с базой данных?" +
               "\nИначе все изменения не сохранятся!", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Authorization form = new Authorization();
                this.Hide();
                form.Show();
            }
        }

        private void вернутьсяКОкнуРегистрацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно сохранили все изменения с базой данных?" +
              "\nИначе все изменения не сохранятся!", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Registration form = new Registration();
                this.Hide();
                form.Show();
            }
        }

        private void обПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа демонстриует возможности работ Windows Forms, C# и MySql." +
                "\nТак же, пример создания базы данных на основе этих инструментов." +
                "\nДанные окно показывает возможность ввода данных и дальнейшую выгрузку их в базу данных.", "Информация об программе!");
        }

        private void выгрузитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Добавить эти данные в базу данных?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseManager _manager = new DatabaseManager();

                try
                {
                    bool add = true;

                    _manager.OpenConnection();//открываем соеденения 

                    //проходим по всем строкам 
                    for (int i = 0; i < dataGrid.Rows.Count; i++)
                    {
                        if (Convert.ToString(this.dataGrid.Rows[i].Cells[1].Value) != "" &&
                        Convert.ToString(this.dataGrid.Rows[i].Cells[2].Value) != "" &&
                        Convert.ToString(this.dataGrid.Rows[i].Cells[3].Value) != "")
                        {
                            string _commandString = "INSERT INTO `projects` (`specification`, `information`, `time_constraints`) " +
                            "VALUES(@specification, @information, @time_constraints)";
                            MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection);//формируем запрос

                            //меняем заглужки на значения
                            _command.Parameters.Add("@specification", MySqlDbType.VarChar).Value = this.dataGrid.Rows[i].Cells[1].Value;
                            _command.Parameters.Add("@information", MySqlDbType.VarChar).Value = this.dataGrid.Rows[i].Cells[2].Value;
                            _command.Parameters.Add("@time_constraints", MySqlDbType.VarChar).Value = Convert.ToString(this.dataGrid.Rows[i].Cells[3].Value);

                            // Здесь наш запрос будет выполнен и данные сохранены в базе данных
                            if (_command.ExecuteNonQuery() != 1)//если хотя бы один не добавился, сообщим об этом
                                add = false;
                        }
                        else
                            MessageBox.Show("Не все поля заполнены!","Внимание!");
                    }

                    if (add)
                        MessageBox.Show("Данные добавлены!", "Внимание!");
                    else
                        MessageBox.Show("Ошибка добавления данных!", "Ошибка!");
                }
                catch
                {
                    MessageBox.Show("Ошибка работы с базой данных!", "Ошибка");
                }
                finally
                {
                    _manager.CloseConnection();
                }                
            }

        }

        private void загрузитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _data.Clear();

            //открываем базу данных и считываем с нее данные
            DatabaseManager _manager = new DatabaseManager();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM `projects`", _manager.GetConnection);
            MySqlDataReader _reader;

            //удаляем все текущие строки
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();

            try
            {
                _manager.OpenConnection();
                _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    //заполняем данные
                    RowOfData row = new RowOfData(_reader["id"], _reader["specification"], _reader["information"], _reader["time_constraints"]);
                    _data.Add(row);
                }

                //добавляем в таблицу данные
                for (int i = 0; i < _data.Count; i++)
                {
                    AddDataGrid(_data[i]);
                    dataGrid.Rows[i].Cells[0].ReadOnly = true;//запрещаем менять id
                }
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
            dataGrid.ReadOnly = false;//разрешаем менять данные
        }

        private void DataUploadForm_Shown(object sender, EventArgs e)
        {
            HeaderOfTheTable();//создаем шапку 
            dataGrid.Columns[0].ReadOnly = true;//запрещаем менять id
            user = new User();//содаем объект класса польователь, так как в нем есть статичная переменная, она будет одинакова для всех 
        }
        
        private void изменитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.userName == "admin" || user.userName == "editor")
            {
                if (MessageBox.Show("Точно изменить эти данные?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (dataGrid.SelectedRows.Count == 0)
                    {
                        //изменяем данные
                        if (Convert.ToString(this.dataGrid.Rows[0].Cells[1].Value) != "" &&
                            Convert.ToString(this.dataGrid.Rows[0].Cells[2].Value) != "" &&
                            Convert.ToString(this.dataGrid.Rows[0].Cells[3].Value) != "")
                        {
                            string id = Convert.ToString(this.dataGrid.Rows[0].Cells[0].Value);
                            string specification = Convert.ToString(this.dataGrid.Rows[0].Cells[1].Value);
                            string information = Convert.ToString(this.dataGrid.Rows[0].Cells[2].Value);
                            string time_constraints = Convert.ToString(this.dataGrid.Rows[0].Cells[3].Value);

                            //открываем базу данных
                            DatabaseManager _manager = new DatabaseManager();
                            string _comanndString = "UPDATE `projects` SET `id` = '" + id + "', " +
                                "`specification` = '" + specification + "', " +
                                "`information` = '" + information + "', " +
                                "`time_constraints` = '" + time_constraints + "' " +
                                "WHERE `projects`.`id` = " + id;
                            MySqlCommand _command = new MySqlCommand(_comanndString, _manager.GetConnection);

                            try
                            {
                                _manager.OpenConnection();
                                _command.ExecuteNonQuery();
                                MessageBox.Show("Данные изменены!", "Внимание!");
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
                        else
                            MessageBox.Show("Не все поля заполнены!", "Внимание!");
                    }
                    else
                    {
                        //открываем базу данных
                        DatabaseManager _manager = new DatabaseManager();
                        _manager.OpenConnection();
                        bool changed = true;

                        for (int i = 0; i < dataGrid.SelectedRows.Count; i++)
                        {
                            if (Convert.ToString(this.dataGrid.SelectedRows[i].Cells[1].Value) != "" &&
                            Convert.ToString(this.dataGrid.SelectedRows[i].Cells[2].Value) != "" &&
                            Convert.ToString(this.dataGrid.SelectedRows[i].Cells[3].Value) != "")
                            {
                                string id = Convert.ToString(this.dataGrid.SelectedRows[i].Cells[0].Value);//так как id не меняются, то в таблице указаны нужные id  
                                string specification = Convert.ToString(this.dataGrid.SelectedRows[i].Cells[1].Value);
                                string information = Convert.ToString(this.dataGrid.SelectedRows[i].Cells[2].Value);
                                string time_constraints = Convert.ToString(this.dataGrid.SelectedRows[i].Cells[3].Value);


                                string _comanndString = "UPDATE `projects` SET `id` = '" + id + "', " +
                                    "`specification` = '" + specification + "', " +
                                    "`information` = '" + information + "', " +
                                    "`time_constraints` = '" + time_constraints + "' " +
                                    "WHERE `projects`.`id` = " + id;
                                MySqlCommand _command = new MySqlCommand(_comanndString, _manager.GetConnection);

                                try
                                {
                                    if (_command.ExecuteNonQuery() != 1)
                                        changed = false;
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка работы с базой данных!", "Ошибка!");
                                }
                            }
                            else
                                MessageBox.Show("Не все поля заполнены!", "Внимание!");
                        }

                        if (changed)
                            MessageBox.Show("Данные изменены!", "Внимание!");
                        else
                            MessageBox.Show("Не все данные изменены!", "Внимание!");

                        _manager.CloseConnection();
                    }
                }
            }
            else
                MessageBox.Show("Ошибка, у вас нет на это доступа! ","Ошибка!");
        }      

        //добавление данных в табицу
        private void AddDataGrid(RowOfData row)
        {
            dataGrid.Rows.Add(row.id, row.secification, row.information, row.time_constraints);//добавляем строку в таблицу
        }
     
        private void удалитьВыбранныйЭлементToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Точно удалить эти данные?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (dataGrid.SelectedRows.Count == 0)
                {
                    int index = Convert.ToInt32(numericUpDown_forSelected.Value);

                    if (index > 0 && index <= _data.Count)
                    {
                        //открываем базу данных
                        DatabaseManager _manager = new DatabaseManager();
                        string id = Convert.ToString(this.dataGrid.Rows[0].Cells[0].Value); 
                        string _comanndString = "DELETE FROM `projects` WHERE `projects`.`id` = " + id;
                        MySqlCommand _command = new MySqlCommand(_comanndString, _manager.GetConnection);

                        try
                        {
                            _manager.OpenConnection();
                            _command.ExecuteNonQuery();
                            MessageBox.Show("Данные удалены!", "Внимание!");
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
                    else
                    {
                        MessageBox.Show("Выбран не верный элемент!", "Ошибка!");
                    }
                }
                else
                {
                    //открываем базу данных
                    DatabaseManager _manager = new DatabaseManager();
                    _manager.OpenConnection();
                    bool delete = true;

                    foreach(DataGridViewRow row in dataGrid.SelectedRows)
                    {
                        string id = Convert.ToString(row.Cells[0].Value);//так как id не меняются, то в таблице указаны нужные id  
                        string _comanndString = "DELETE FROM `projects` WHERE `projects`.`id` = " + id;
                        MySqlCommand _command = new MySqlCommand(_comanndString, _manager.GetConnection);

                        try
                        {
                            dataGrid.Rows.Remove(row);
                            if (_command.ExecuteNonQuery() != 1)
                                delete = false;
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка работы с базой данных!", "Ошибка!");
                        }
                    }

                    if(delete)
                        MessageBox.Show("Данные удалены!", "Внимание!");
                    else
                        MessageBox.Show("Не все данные удалены!", "Внимание!");

                    _manager.CloseConnection();
                }
            }
        }

        private void удалитьВсеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.userName == "admin")
            {
                if (MessageBox.Show("Точно удалить все данные?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //удаляем данные

                    //открываем базу данных и считываем с нее данные
                    DatabaseManager _manager = new DatabaseManager();
                    MySqlCommand _command = new MySqlCommand("TRUNCATE TABLE `projects`", _manager.GetConnection);

                    try
                    {
                        _manager.OpenConnection();

                        //выполянем запрос
                        _command.ExecuteNonQuery();
                        MessageBox.Show("Данные удалены!", "Внимание!");
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления данныех!", "Ошибка!");
                    }
                    finally
                    {
                        _manager.CloseConnection();
                    }
                }
            }
            else
                MessageBox.Show("Ошибка, у вас нет на это доступа! ", "Ошибка!");
        }

        private void перейтиВОкноПросмотраДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Перейти в окно загрузки данных?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataForm form = new DataForm();
                this.Hide();
                form.Show();
            }
        }

        private void Create_Click(object sender, EventArgs e)
        {
            //удаляем все текущие строки
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();

            dataGrid.RowCount = Convert.ToInt32(numericUpDown_forAddData.Value);//считываем количество строк
            dataGrid.ReadOnly = false;//разрешаем менять данные 

            for(int i = 0; i<dataGrid.Rows.Count; i++)
                dataGrid.Rows[i].Cells[0].ReadOnly = true;//запрещаем менять id 
        }

        private void Choose_Click(object sender, EventArgs e)
        {
            //удаляем все текущие строки
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();

            _data.Clear();
            
            //открываем базу данных
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
                    RowOfData row = new RowOfData(_reader["id"], _reader["specification"], _reader["information"], _reader["time_constraints"]);
                    _data.Add(row);
                }

                //добавляем в таблицу данные
                int i = Convert.ToInt32(numericUpDown_forSelected.Value) - 1;

                if (i >= 0 && i < _data.Count)
                {
                    AddDataGrid(_data[i]);
                }
                else
                    MessageBox.Show("Выбран не правильный элемент!", "Ошибка!");

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
    }
}
