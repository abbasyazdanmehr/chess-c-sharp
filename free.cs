using System;
using System.Collections.Generic;

namespace Free
{
    public class Test
    {
        static void Main()
        {
            // System.Console.WriteLine((int)('A'));
            Person person = new Person("akbar", "1");
            Person person2 = new Person("mohammd", "2");
            Person person3 = new Person("somayyeh", "4");
            List<Person> persons = new List<Person>();
            persons.Add(person);
            persons.Add(person2);
            persons.Add(person3);

            Person person4 = new Person("akbar", "1");
            System.Console.WriteLine(persons.Contains(person4));
        }

        public static void print_something()
        {
            System.Console.WriteLine("something");
        }
    }

    public class Person
    {
        public string name = "ali";
        public string password = "3";

        public Person(string inName, string inPassword)
        {
            name = inName;
            password = inPassword;
        }
    }
}
