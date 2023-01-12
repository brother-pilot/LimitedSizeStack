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
        //public List<TItem> Items { get; }
        //public int Limit;
        //public LimitedSizeStack<List<object>> History { get; }
        Invoker invoker;
        Receiver receiver;

       public ListModel(int limit)
        {
            //History= new LimitedSizeStack<List<object>>(limit);
            //Items = new List<TItem>();
            //Limit = limit;
            invoker = new Invoker();
            receiver = new Receiver(limit);
            invoker.SetCommand(new ConcreteCommand(receiver, limit));
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            //History.Push(new List<object> { "add", item });
            //invoker.Add(item);
            invoker.Execute(new List<object> { "add", item });
        }

        public void RemoveItem(int index)
        {
            //History.Push(new List<object> { "rem", index, Items[index] });
            Items.RemoveAt(index);
            //invoker.RemoveAt(index);
            invoker.Execute(new List<object> { "rem", index, Items[index] });
        }

        public bool CanUndo()//возвращает true, если на данный момент история действий не пуста
        {
            //return false;
            return invoker.CanUndo();
        }


    }

    interface ICommand
    {
        void Execute();
        void UnExecute();
    }

    class ConcreteCommand : ICommand
    {
        Receiver receiver; //подключаем исполнителя команды
        public LimitedSizeStack<List<object>> History { get; }
       

        public ConcreteCommand(Receiver r, int limit) //указваем какой конкретно исполнитель
        {
            receiver = r;
            History = new LimitedSizeStack<List<object>>(limit);
        }
        public void Execute(List<object> obj) //string str, TItem item) //прописана реализация команды execute т.е. чего конктретно то делаем
        {
            History.Push(obj);
            //receiver.Add();
        }

        public void UnExecute() //прописана реализация команды Undo т.е. чего конктретно то делаем
        {
            receiver.Off();
        }

        public bool CanUndo()//возвращает true, если на данный момент история действий не пуста
        {
            //return false;
            return History.Count > 0;
        }



        public void Undo() //отменяет последнее действие из истории
        {

            var command = History.Pop();
            if (command[0].ToString() == "rem")
                Items.Insert((int)command[1], (TItem)command[2]);
            if (command[0].ToString() == "add")
                Items.RemoveAt(Items.Count - 1);
        }

    }

    // Receiver - получатель команды. Определяет действия, которые должны выполняться в результате запроса
    class Receiver
    {
        public List<TItem> Items { get; }
        public int Limit;
        TItem item = new ListModel<TItem>();
        public Receiver(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
        }
        public void Add(TItem item)
        {
            Items.Add(item); ;
        }

        public void Remove(int index)
        {
            Items.RemoveAt(index);
        }
    }
    // Invoker - инициатор команды- вызывает команду для выполнения определенного запроса
    class Invoker
    {
        ICommand command;//завязываем какие у нас команды будут выполняться. По идее здесь должны быть расписаны все команды по отдельности
        public void SetCommand(ICommand c)
        {
            command = c;
        }
        public void Execute(List<object> obj)
        {
            command.Execute(obj);
        }
        public void CanUndo(List<object> obj)
        {
            command.CanUndo();
        }
    }


    class Client
    {
        static void Main() //клиент - создает команду и устанавливает ее получателя с помощью метода SetCommand()
                           //есть инициатор команды - класс Pult, некий прибор - пульт, управляющий телевизором.И есть получатель команды - класс TV,
                           //представляющий телевизор. В качестве клиента используется класс Clien
        {
            Pult invoker = new Pult();// подключаем инициатор через которого будем посылать команду
            TV receiver = new TV();// подключаем конечного выполнителя команды
            ConcreteCommand command = new ConcreteCommand(receiver);//передаем данные что команды будут отрабатываться именно на
                                                                    //таком выполнителе - телевизоре/
                                                                    //связывает объекты исполнителей с созданными командами (инкапсулируем исполнителя)
            invoker.SetCommand(command);//связывает объекты отправителей с созданными командами (инкапсулируем команды)
            //по идее здесь должен писаться полный список команд с параметрами запуска как для микроволновки, а не одна команда
            //две верхних строки понятней было бы написать так invoker.SetCommand(new ConcreteCommand(receiver));
            invoker.PressButtonRun();//команда на включение телевизора
            invoker.PressButtonCancel();

        }
    }
