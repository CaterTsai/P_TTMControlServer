using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttmControlServer.Models
{
    public class questionData
    {
        public const int rowUnitNum = 4;
        public const int colUnitNum = 12;
        public const int rowLength = rowUnitNum * 4;
        public const int colLength = colUnitNum * 4;
        public const int dataLenght = rowLength * colLength;
        private bool[] question { get; set; }
        public questionData()
        {
            question = new bool[dataLenght];
        }

        public void setQuestion(string data)
        {
            int i = 0;
            foreach (char c in data)
            {
                if(c == '0')
                {
                    question[i] = false;
                }
                else
                {
                    question[i] = true;
                }
                i++;
            }
        }

        public void getAnswer(int index, ref bool[] answerList)
        {
            if(answerList.Count() != 16)
            {
                return;
            }
            int x = (index % colUnitNum) * 4;
            int y = ((int)(index / (float)colUnitNum)) * 4; 

            for(int sy = 0; sy < 4; sy++)
            {
                int ty = y + sy;
                for(int sx = 0; sx < 4; sx++)
                {
                    int tx = x + sx;
                    int sIdx = ty * colLength + tx;
                    int tIdx = sy * 4 + sx;

                    answerList[tIdx] = question[sIdx];
                }
            }
        }
    }
}