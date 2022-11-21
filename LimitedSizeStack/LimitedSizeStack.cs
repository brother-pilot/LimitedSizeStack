
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * В этой задаче вам нужно реализовать стек ограниченного размера. Этот стек работает как обычный стек, однако при превышении максимального размера удаляет самый глубокий элемент в стеке. Таким образом в стеке всегда будет ограниченное число элементов.

Вот пример работы такого стека с ограничением в 2 элемента:

// сначала стек пуст
stack.Push(10); // в стеке 10
stack.Push(20); // в стеке 10, 20
stack.Push(30); // в стеке 20, 30
stack.Push(40); // в стеке 30, 40
stack.Pop(); // возвращает 40, в стеке остаётся 30
stack.Pop(); // возвращает 30, стек после этого пуст

Операция Push должна иметь сложность O(1), то есть никак не зависеть от размера стека.
Реализуйте класс LimitedSizeStack.

Отладьте его реализацию с помощью тестов в классе LimitedSizeStack_should. Проверьте эффективность операции Push с помощью теста из класса LimitedSizeStack_PerformanceTest.*/

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        public readonly int Limit;
        public List<T> items;

        public LimitedSizeStack(int limit)
        {
            Limit = limit;
        }

        public void Push(T item)
        {
            if (Count < Limit) 
            { 
                this.items.Add(item);
            }
            else
            {
                this.items.RemoveAt(0);
                this.items.Add(item);
            }
        }

        public T Pop()
        {
            T result;
            if (Count != null)
            {
                result = items[Count - 1];
                this.items.RemoveAt(Count - 1);
            }
            else
                result = default(T); 
            return result;
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }
    }
}