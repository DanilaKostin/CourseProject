using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_on_forms_beta1
{
    public class Queue
    {
        public Queue()
        {
            PriorityQueue = new List<Tuple<Vertex, double>>();
        }

        readonly List<Tuple<Vertex, double>> PriorityQueue;

        public void Insert(Tuple<Vertex, double> v)
        {
            PriorityQueue.Add(v);
            int i = PriorityQueue.Count;
            while ((i > 1) && (PriorityQueue[i - 1].Item2 < PriorityQueue[i / 2 - 1].Item2))
            {
                var temp = PriorityQueue[i - 1];
                PriorityQueue[i - 1] = PriorityQueue[i / 2 - 1];
                PriorityQueue[i / 2 - 1] = temp;
                i /= 2;
            }
        }

        public Vertex DeleteMin()
        {
            if (PriorityQueue.Count == 0)
            {
                return null;
            }
            int i = 1, j;
            var x = PriorityQueue[0].Item1;
            PriorityQueue[0] = PriorityQueue[PriorityQueue.Count - 1];
            PriorityQueue.RemoveAt(PriorityQueue.Count - 1);
            while ((i <= PriorityQueue.Count / 2))
            {
                if (2 * i == PriorityQueue.Count || PriorityQueue[2 * i - 1].Item2 < PriorityQueue[2 * i].Item2)
                    j = 2 * i;
                else j = 2 * i + 1;
                if (PriorityQueue[i - 1].Item2 > PriorityQueue[j - 1].Item2)
                {
                    var temp = PriorityQueue[i - 1];
                    PriorityQueue[i - 1] = PriorityQueue[j - 1];
                    PriorityQueue[j - 1] = temp;
                    i = j;
                }
                else break;
            }
            return x;
        }
    }
}







/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_on_forms_beta1
{
    public class Queue
    {
        /// <summary>
        /// Конструктор класса Queue
        /// </summary>
        public Queue()
        {
            PriorityQueue = new List<Vertex>();
        }

        /// <summary>
        /// Поле, реализующее двоичную кучу
        /// </summary>
        readonly List<Vertex> PriorityQueue;

        /// <summary>
        /// Метод вставки в двоичную кучу элемента Vertex
        /// </summary>
        /// <param name="v">Вершина Vertex</param>
        /// <exception cref="ArgumentOutOfRangeException">При не установке в переданном агрументе поля Cost 
        /// (см. <see cref="Vertex"/>) функция не может производить сортировку в двоичной куче</exception>
        public void Insert(Vertex v)
        {
            PriorityQueue.Add(v);
            int i = PriorityQueue.Count;
            while ((i > 1) && (PriorityQueue[i - 1].Cost < PriorityQueue[i / 2 - 1].Cost))
            {
                Vertex temp = PriorityQueue[i - 1];
                PriorityQueue[i - 1] = PriorityQueue[i / 2 - 1];
                PriorityQueue[i / 2 - 1] = temp;
                i /= 2;
            }
        }
        /// <summary>
        /// Метод, возвращающий минимум из очереди
        /// </summary>
        /// <returns>Вершина Vertex</returns>
        public Vertex DeleteMin()
        {
            if (PriorityQueue.Count == 0)
            {
                //Console.WriteLine("Очередь пуста");
                return null;
            }
            int i = 1, j;
            Vertex x = PriorityQueue[0];
            PriorityQueue[0] = PriorityQueue[PriorityQueue.Count - 1];
            PriorityQueue.RemoveAt(PriorityQueue.Count - 1);
            while ((i <= PriorityQueue.Count / 2))
            {
                if (2 * i == PriorityQueue.Count || PriorityQueue[2 * i - 1].Cost < PriorityQueue[2 * i].Cost)
                    j = 2 * i;
                else j = 2 * i + 1;
                if (PriorityQueue[i - 1].Cost > PriorityQueue[j - 1].Cost)
                {
                    Vertex temp = PriorityQueue[i - 1];
                    PriorityQueue[i - 1] = PriorityQueue[j - 1];
                    PriorityQueue[j - 1] = temp;
                    i = j;
                }
                else break;
            }
            return x;
        }
    }
}
*/