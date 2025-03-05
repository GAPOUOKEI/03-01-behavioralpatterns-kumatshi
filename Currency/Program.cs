using System;

namespace Currency
{
    /*
* ФИО студента: Тазеев Марс Эдуардович
* номер варианта: 4 
* условие задачи (скопировать из листа задания): Напишите программу, которая показывает курсы валют и меняет свое 
* состояние зависимости от того, активен ли режим обновления или просмотра сохраненных
*/

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в просмотр курсов валют!");
            var viewer = new CurrencyViewer();

            
            Console.WriteLine("\nШаг 1: Показ сохраненных данных");
            viewer.ShowRates();

            
            Console.WriteLine("\nШаг 2: Переключение режима");
            viewer.ToggleMode();
            viewer.ShowRates();

            
            Console.WriteLine("\nШаг 3: Возврат в оффлайн");
            viewer.ToggleMode();
            viewer.ShowRates();

            
            Console.WriteLine("\nШаг 4: Повторный переход в онлайн");
            viewer.ToggleMode();
            viewer.ShowRates();
        }
    }
    class CurrencyViewer
    {
        private ICurrencyState _state;

        public CurrencyViewer()
        {
            _state = new OfflineState(this);
        }

        public void SetState(ICurrencyState state) => _state = state;
        public void ShowRates() => _state.ShowRates();
        public void ToggleMode() => _state.ToggleMode();
    }

    
    interface ICurrencyState
    {
        void ShowRates();
        void ToggleMode();
    }

    
    class OfflineState : ICurrencyState
    {
        private CurrencyViewer _viewer;
        private string _savedRates = "USD: 91.50 RUB\nEUR: 99.20 RUB\nGBP: 120.30 RUB";

        public OfflineState(CurrencyViewer viewer)
        {
            _viewer = viewer;
        }

        public void ShowRates()
        {
            Console.WriteLine("=== Режим оффлайн ===");
            Console.WriteLine("Сохраненные курсы валют:");
            Console.WriteLine(_savedRates);
            Console.WriteLine("Данные от 05.03.2025");
        }

        public void ToggleMode()
        {
            _viewer.SetState(new OnlineState(_viewer));
            Console.WriteLine("Переключение в режим онлайн...");
        }
    }

    
    class OnlineState : ICurrencyState
    {
        private CurrencyViewer _viewer;

        public OnlineState(CurrencyViewer viewer)
        {
            _viewer = viewer;
        }

        public void ShowRates()
        {
            Console.WriteLine("=== Режим онлайн ===");
            Console.WriteLine("Получение актуальных курсов валют...");
            
            Console.WriteLine("USD: 92.10 RUB");
            Console.WriteLine("EUR: 100.30 RUB");
            Console.WriteLine("GBP: 121.50 RUB");
            Console.WriteLine($"Данные на {DateTime.Now:dd.MM.yyyy HH:mm}");
        }

        public void ToggleMode()
        {
            _viewer.SetState(new OfflineState(_viewer));
            Console.WriteLine("Переключение в режим оффлайн...");
        }
    }



}