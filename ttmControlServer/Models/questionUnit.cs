using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttmControlServer.Models
{
    
    public class questionUnit
    {
        public const int questionLength = 16;
        public questionUnit()
        {
            question = new bool[questionLength];
            flag = new bool();
            reset();
        }

        public void reset()
        {
            for(int i = 0; i < question.Count(); i++)
            {
                question[i] = false;
            }
            flag = false;
        }

        public void randomSet()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < questionLength; i++)
            {
                question[i] = (r.Next() % 2 == 0);
            }
        }
        public bool[] question { get; set; }

        public bool flag { get; set;}
    }
}