using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лучшее_текстовое_РПГ_всех_времен_и_народов
{
    public class Savespot
    {
        public int number;
        public string name;
        public int movies;
        public int points;

        public bool search_by_number(int number)
        {
            if(this.number==number)
            {
                return true;
            }
            return false;
        }
    }
}
