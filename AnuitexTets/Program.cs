using System;
using System.Collections.Generic;
using System.Linq;

namespace AnuitexTets
{

    public static class Extensions
    {
        public static bool DoswFirmHaveEmploy(this Firm firm, Employee employee)
        {
            var x = firm.GetAllObjectsOnEmployees<Employee>().Where(v => v==employee).Count();
            return x != 0 ? true: false ;
        }
    }

    public static class LINQExtension
    {
        public static void DisplayAllEmployees(this IEnumerable<Employee>? employees)
        {
            if (!(employees?.Any() ?? false))
            {
                throw new InvalidOperationException("Cannot show all employees for a null or empty set.");
            }

            foreach (var x in employees)
            {
                Console.WriteLine(x.Name + " " + x.Surname + " " + x.Patronymic + " " + x.Experience);
            }
        }
    }



    public abstract class Employee
    {
        public double Experience { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronymic { get; }

        public Employee(string Name, string Surname, string Patronymic, double Experience)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Patronymic = Patronymic;
            this.Experience = Experience;
        }

        public virtual void DoWork() { }

        public virtual void UniqueWork() { }
    }

    public class Workman : Employee
    {
        public Workman(string Name, string Surname, string Patronymic, double Experience) 
            : base(Name, Surname, Patronymic, Experience)
        {}

        public override void DoWork()
        {
            Console.WriteLine("Выпуск продукции");
        }
    }

    public class Manager : Employee
    {
        public Manager(string Name, string Surname, string Patronymic, double Experience)
            : base(Name, Surname, Patronymic, Experience)
        { }

        public override void DoWork()
        {
            Console.WriteLine("Сбор заказов");
        }

        public override void UniqueWork()
        {
            Console.WriteLine("Задание выдано");
        }
    }

    public class Brigadier : Employee
    {
        public Brigadier(string Name, string Surname, string Patronymic, double Experience)
            : base(Name, Surname, Patronymic, Experience)
        { }

        public override void DoWork()
        {
            Console.WriteLine("Закупка материалов");
        }

        public override void UniqueWork()
        {
            Console.WriteLine("Все рабочте на месте)");
        }
    }



    public class Firm
    {
        private List<Employee> employees = new List<Employee>();

        public List<T> GetAllObjectsOnEmployees<T>() where T : Employee
        {
            List<T> result = new List<T>();

            for (int i = 0; i < employees.Count; i++)
            {
                    if (employees[i] is T) 
                        result.Add(employees[i] as T);
            }

            return result;
        }

        public int CountAllObjectsOnEmployees<T>() where T : Employee
        {
            List<T> result = new List<T>();

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i] is T) 
                    result.Add(employees[i] as T);
            }

            return result.Count;
        }

        public static Firm operator +(Firm firm, Employee employee)
        {

            firm.employees.Add(employee);
            return firm;
        }

        public static Firm operator -(Firm firm, Employee employee)
        {
            var fm = from t in firm.employees
                     where t != employee
                     select t;


            firm.employees = fm.ToList();

            return firm;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Firm fm = new Firm();

            Workman wm1 = new Workman("Slava","Grinevich","Dmitrievich", 3);
            Workman wm2 = new Workman("Marina", "Bondarenka", "Evginivna", 1.5);
            Workman wm3 = new Workman("Sergey", "Nedostup", "Viacheslavovich", 0.5);

            Manager m1 = new Manager("Marina", "Grinevich", "Viacheslavovich", 6.2);
            Manager m2 = new Manager("Slava", "Nedostup", "Evginivna", 4.2);
            Manager m3 = new Manager("Sergey", "Bondarenka", "Dmitrievich", 3.1);

            Brigadier b1 = new Brigadier("Marina", "Nedostup", "Viacheslavovna", 6.2);
            Brigadier b2 = new Brigadier("Slava", "Grinevich", "Viacheslavovich", 6.0);
            Brigadier b3 = new Brigadier("Sergey", "Bondarenka", "Evginivich", 0.9);

            fm = fm + wm1;
            fm = fm + wm2;
            fm = fm + wm3;

            fm = fm + m1;
            fm = fm + m2;
            fm = fm + m3;

            fm = fm + b1;
            fm = fm + b2;
            fm = fm + b3;

            foreach (var x in fm.GetAllObjectsOnEmployees<Employee>())
            {
                Console.WriteLine(x.Name + " " + x.Surname + " " + x.Patronymic + " " + x.Experience);
                x.UniqueWork();
                x.DoWork();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (fm.DoswFirmHaveEmploy(wm1))
                Console.WriteLine("Работник " + wm1.Name + " " + wm1.Surname + " числиться в фирме");
            else
                Console.WriteLine("Работник " + wm1.Name + " " + wm1.Surname + " не числиться в фирме");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            fm = fm - wm1;
            fm = fm - wm3;

            fm.GetAllObjectsOnEmployees<Employee>().DisplayAllEmployees();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (fm.DoswFirmHaveEmploy(wm1))
                Console.WriteLine("Работник " + wm1.Name + " " + wm1.Surname + " числиться в фирме");
            else
                Console.WriteLine("Работник " + wm1.Name + " " + wm1.Surname + " не числиться в фирме");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Counts Workman on Firms: " + fm.CountAllObjectsOnEmployees<Workman>());
            Console.WriteLine("All Workman on Firms");
            fm.GetAllObjectsOnEmployees<Brigadier>().DisplayAllEmployees();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Counts Brigadier on Firms: " + fm.CountAllObjectsOnEmployees<Brigadier>());
            Console.WriteLine("All Brigadier on Firms");

            fm.GetAllObjectsOnEmployees<Brigadier>().DisplayAllEmployees();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
