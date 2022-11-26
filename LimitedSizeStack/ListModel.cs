using System;
using System.Collections.Generic;

/*
 * Если вы запустите проект на исполнение, то увидете окно приложения, в котором можно добавлять новые дела и удалять уже 
 * существующие. Однако кнопка "Отмена" пока не работает. Ваша задача — сделать так, чтобы эта кнопка отменяла последнее 
 * действие пользователя.

Изучите класс ListModel — в нём реализована логика работы кнопок в приложении.

Реализуйте в методы Undo и CanUndo. Для этого нужно хранить историю последних limit действий удаления/добавления. 
Используйте для этого класс LimitedSizeStack из прошлой задачи.

Метод Undo отменяет последнее действие из истории.
Метод CanUndo возвращает true, если на данный момент история действий не пуста, то есть если вызов Undo будет корректным. 
Иначе метод должен вернуть false.
Проверить корректность своего решения можно на модульных тестах из класса ListModel_Should и ListModel_PerformanceTest.
*/

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        public LimitedSizeStack<List<object>> History { get; }

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            History= new LimitedSizeStack<List<object>>(limit);
            Limit = limit;
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            History.Push(new List<object> { "add", item });
        }

        public void RemoveItem(int index)
        {
            History.Push(new List<object> { "rem", index, Items[index] });
            Items.RemoveAt(index);
        }

        public bool CanUndo()//возвращает true, если на данный момент история действий не пуста
        {
            return History.Count>0;
        }

        public void Undo() //отменяет последнее действие из истории
        {

            var command = History.Pop();
            if (command[0].ToString() == "rem")
                Items.Insert((int)command[1], (TItem)command[2]);
            if (command[0].ToString() == "add")
                Items.RemoveAt(Items.Count-1);
        }
    }
}
