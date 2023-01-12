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
Если хотите, можете воспользоваться классическим объектно-ориентированным шаблоном Команда. Однако для сдачи данной задачи, точно следовать этому шаблону необязательно

 */

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        Invoker<TItem> invoker;
        Receiver<TItem> receiver;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            invoker = new Invoker<TItem>();
            receiver = new Receiver<TItem>(Items);
            invoker.SetCommand(new ConcreteCommand<TItem>(receiver, limit));
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            invoker.Execute(new List<object> { "add", item });
        }

        public void RemoveItem(int index)
        {
            invoker.Execute(new List<object> { "rem", index, Items[index] });
            Items.RemoveAt(index);
        }

        public bool CanUndo()//возвращает true, если на данный момент история действий не пуста
        {
            return invoker.CanUndo();
        }

        public void Undo() //отменяет последнее действие из истории
        {
            invoker.Undo();
        }


    }

    interface ICommand<TItem>
    {
        void Execute(List<object> obj);
        bool CanUndo();
        void Undo();
    }

    class ConcreteCommand<TItem> : ICommand<TItem>
    {
        Receiver<TItem> receiver ; //подключаем исполнителя команды
        public LimitedSizeStack<List<object>> History { get; }


        public ConcreteCommand(Receiver<TItem> r, int limit) //указваем какой конкретно исполнитель
        {
            receiver = r;
            History = new LimitedSizeStack<List<object>>(limit);
        }
        public void Execute(List<object> obj) //string str, TItem item) //прописана реализация команды execute т.е. чего конктретно то делаем
        {
            History.Push(obj);
        }

        public bool CanUndo()//возвращает true, если на данный момент история действий не пуста
        {
            return History.Count > 0;
        }
        public void Undo() //отменяет последнее действие из истории
        {

            var command = History.Pop();
            if (command[0].ToString() == "rem")
                receiver.Insert((int)command[1], command[2]);
            if (command[0].ToString() == "add")
                receiver.Remove();
        }

    }

    // Receiver - получатель команды. Определяет действия, которые должны выполняться в результате запроса
    class Receiver<TItem>
    {
        public List<TItem> Items { get; }
        public Receiver(List<TItem> items)
        {
            Items = items;
        }
        public void Insert(int position, object item)
        {
            Items.Insert(position, (TItem)item);
        }

        public void Remove()
        {
            Items.RemoveAt(Items.Count - 1);
        }
    }
    // Invoker - инициатор команды- вызывает команду для выполнения определенного запроса
    class Invoker<Titem>
    {
        ICommand<Titem> command;//завязываем какие у нас команды будут выполняться. По идее здесь должны быть расписаны все команды по отдельности
        public void SetCommand(ICommand<Titem> c)
        {
            command = c;
        }
        public void Execute(List<object> obj)
        {
            command.Execute(obj);
        }
        public bool CanUndo()
        {
            return command.CanUndo();
        }

        public void Undo() //отменяет последнее действие из истории
        {
            command.Undo();
        }
    }


}    
