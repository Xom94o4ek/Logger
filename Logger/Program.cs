using System;
using System.Collections.Generic;
using System.IO;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger Log = new Logger();
            int toExit = 1;
            while (toExit != 0)
            {
                Console.WriteLine("Введите делитель:");
                int c = Convert.ToInt32(Console.ReadLine());
                try
                {
                    int x = 5;
                    int y = x / c;
                    Console.WriteLine($"Результат: {y}");
                    Console.WriteLine("Если хотите повторить деление на другое число, то введите любое число кроме \"0\"");
                    toExit = Convert.ToInt32(Console.ReadLine());
                    Log.Fatal($"Все без фатальных ошибок c = {c}");
                    Log.Error($"Все без ошибок c = {c}");
                    Log.Warning($"Все без предупреждений c = {c}");
                    Log.Info($"Просто информация c = {c}");
                    Log.Info($"Просто информация со значениями c = {c}",x,y,c,toExit);
                    Log.Debug($"Просто Debug c = {c}");
                    Log.DebugFormat($"Просто Debug со значениями c = {c}", x, y, c, toExit);
                    Dictionary<object, object> ForTest = new Dictionary<object, object>();
                    ForTest.Add("x", x);
                    ForTest.Add("y", y);
                    ForTest.Add("c", c);
                    ForTest.Add("Состояние Выхода", toExit);
                    Log.SystemInfo($"Просто SysInfo с объектами c = {c}", ForTest);
                    Log.SystemInfo("Просто SysInfo без объектов");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка деления!");
                    Log.ErrorUnique($"(1)Ошибка уникальная = {c}", ex);
                    Log.Fatal($"Фатальная ошибка = {c}",ex);
                    Log.Error($"Ошибка = {c}",ex);
                    Log.Error(ex);
                    Log.ErrorUnique($"(2)Ошибка уникальная = {c}", ex);
                    Log.WarningUnique($"Предупреждение = {c}");
                    Log.Warning($"Предупреждение = {c}");
                    Log.WarningUnique($"Предупреждение = {c}");
                    Log.Info($"Просто информация с ошибкой c = {c}",ex);
                    Log.Debug($"Просто Debug с ошибкой c = {c}");
                    /*Console.WriteLine($"Исключение: {ex.Message}");
                    Console.WriteLine($"Метод: {ex.TargetSite}");
                    Console.WriteLine($"Трассировка стека: {ex.StackTrace}");*/
                    Console.WriteLine("Если хотите повторить деление на другое число, то введите любое число кроме \"0\"");
                    toExit = Convert.ToInt32(Console.ReadLine());
                }
            }
            Console.ReadLine();
        }
    }
}
