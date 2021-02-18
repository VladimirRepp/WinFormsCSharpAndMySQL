using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPS.DataBase
{
    /*
     * Этот класс будет описывать одну строку данных
     * Так будет удобнее передавать в метод добавления данных
     */
    class RowOfData
    {
        //тут перечисляем поля, которые указаны в базе данных
        public object id { get; set; }
        public object secification { get; set; }
        public object information { get; set; }
        public object time_constraints { get; set; }

        //конструкторы класса
        public RowOfData() { }
        public RowOfData(object _id, object _secification, object _information, object _time_constraints)
        {
            id = _id;
            secification = _secification;
            information = _information;
            time_constraints = _time_constraints;
        }
        
        //методы класса
        public void DataChange(object _id, object _secification, object _information, object _time_constraints)
        {
            id = _id;
            secification = _secification;
            information = _information;
            time_constraints = _time_constraints;
        }
    }
}
