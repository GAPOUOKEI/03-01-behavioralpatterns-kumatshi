using System;
using System.Collections.Generic;

namespace ContactManagerApp
{
    /*
 * ФИО студента: Тазеев Марс Эдуардович
 * номер варианта: 4 
 * условие задачи (скопировать из листа задания): Создайте приложение для хранения контактов, 
 * которое позволяет добавлять, редактировать и удалять контакты с помощью команд, а также поддерживает отмену этих действий.
 */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в менеджер контактов!");
            var app = new ContactApp();

            
            Console.WriteLine("\n=== Добавление контактов ===");
            var addCmd1 = new AddContactCommand(app.GetManager(), new Contact { Name = "Иван", Phone = "+7-999-123-45-67" });
            var addCmd2 = new AddContactCommand(app.GetManager(), new Contact { Name = "Мария", Phone = "+7-999-987-65-43" });
            app.ExecuteCommand(addCmd1);
            app.ExecuteCommand(addCmd2);
            app.ShowContacts();

            
            Console.WriteLine("=== Редактирование контакта ===");
            var editCmd = new EditContactCommand(app.GetManager(), 0, new Contact { Name = "Иван Иванов", Phone = "+7-999-123-45-67" });
            app.ExecuteCommand(editCmd);
            app.ShowContacts();

            
            Console.WriteLine("=== Удаление контакта ===");
            var removeCmd = new RemoveContactCommand(app.GetManager(), app.GetManager().GetContact(1));
            app.ExecuteCommand(removeCmd);
            app.ShowContacts();

            
            Console.WriteLine("=== Отмена действий ===");
            app.Undo(); 
            app.ShowContacts();
            app.Undo(); 
            app.ShowContacts();
            app.Undo(); 
            app.ShowContacts();
        }
    }
    class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public override string ToString() => $"{Name}: {Phone}";
    }

    
    interface ICommand
    {
        void Execute();
        void Undo();
    }

    
    class ContactManager
    {
        private List<Contact> _contacts = new List<Contact>();

        public void AddContact(Contact contact)
        {
            _contacts.Add(contact);
            Console.WriteLine($"Контакт добавлен: {contact}");
        }

        public void RemoveContact(Contact contact)
        {
            _contacts.Remove(contact);
            Console.WriteLine($"Контакт удален: {contact}");
        }

        public void EditContact(int index, Contact newContact)
        {
            _contacts[index] = newContact;
            Console.WriteLine($"Контакт изменен на: {newContact}");
        }

        public void ShowContacts()
        {
            Console.WriteLine("\nСписок контактов:");
            if (_contacts.Count == 0)
                Console.WriteLine("Список пуст");
            else
                _contacts.ForEach(c => Console.WriteLine($"- {c}"));
            Console.WriteLine();
        }

        public Contact GetContact(int index) => _contacts[index];
        public int ContactCount => _contacts.Count;
    }

   
    class AddContactCommand : ICommand
    {
        private ContactManager _manager;
        private Contact _contact;

        public AddContactCommand(ContactManager manager, Contact contact)
        {
            _manager = manager;
            _contact = contact;
        }

        public void Execute() => _manager.AddContact(_contact);
        public void Undo() => _manager.RemoveContact(_contact);
    }

    
    class RemoveContactCommand : ICommand
    {
        private ContactManager _manager;
        private Contact _contact;

        public RemoveContactCommand(ContactManager manager, Contact contact)
        {
            _manager = manager;
            _contact = contact;
        }

        public void Execute() => _manager.RemoveContact(_contact);
        public void Undo() => _manager.AddContact(_contact);
    }

    
    class EditContactCommand : ICommand
    {
        private ContactManager _manager;
        private int _index;
        private Contact _newContact;
        private Contact _oldContact;

        public EditContactCommand(ContactManager manager, int index, Contact newContact)
        {
            _manager = manager;
            _index = index;
            _newContact = newContact;
        }

        public void Execute()
        {
            _oldContact = _manager.GetContact(_index);
            _manager.EditContact(_index, _newContact);
        }

        public void Undo() => _manager.EditContact(_index, _oldContact);
    }

   
    class ContactApp
    {
        private ContactManager _manager;
        private Stack<ICommand> _history = new Stack<ICommand>();

        public ContactApp()
        {
            _manager = new ContactManager();
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                Console.WriteLine("Отмена последнего действия:");
                _history.Pop().Undo();
            }
            else
            {
                Console.WriteLine("Нет действий для отмены");
            }
        }

        public void ShowContacts() => _manager.ShowContacts();

        
        public ContactManager GetManager() => _manager;
    }

    
}