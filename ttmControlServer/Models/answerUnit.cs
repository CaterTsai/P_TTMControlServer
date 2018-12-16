using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttmControlServer.Models
{
    
    public class answerUnit
    {
        public const int answerLength = 16;
        public answerUnit()
        {
            answer = new bool[answerLength];
            flag = new bool();
            isAnswer = new bool();
            reset();
        }

        public void reset()
        {
            for(int i = 0; i < answer.Count(); i++)
            {
                answer[i] = false;
            }
            flag = false;
            isAnswer = false;
        }

        public void set(ref bool[] data)
        {
            if(data.Count() != answerLength)
            {
                return;
            }

            for (int i = 0; i < answerLength; i++)
            {
                answer[i] = data[i];
            }
        }

        public void randomSet()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < answerLength; i++)
            {
                answer[i] = (r.Next() % 2 == 0);
            }
        }
        public bool[] answer { get; set; }

        public bool flag { get; set;}

        public bool isAnswer { get; set; }
    }
}