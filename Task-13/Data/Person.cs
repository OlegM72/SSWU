using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task13
{
    internal enum PersonStatus  // the status also defines priority:
                                // the highest priority has LOWEST value
    {
        Disabled = 0, // people with disabilities, should be served first
        Pensioner,    // people with the age >= 65
        Child,        // people with the age < 18
        Ordinary
    }

    internal class Person
    {
        public Guid Id { get; }
        string name;
        int serviceTime;
        int age;
        double coordinate;
        PersonStatus status;

        public int ServiceTime {
            get => serviceTime;
            set => serviceTime = value; }

        public double Coordinate { get => coordinate; }

        public PersonStatus Status { get => status; }

        public Person() : this(PersonStatus.Ordinary, "", default, default, default) { }

        public Person(PersonStatus status, string name, int age, double coordinate, int serviceTime)
        {
            Id = Guid.NewGuid();
            this.name = name;
            this.age = age;
            this.coordinate = coordinate;
            this.status = status;
            this.serviceTime = serviceTime;
        }

        public string ShowForReport() // a string representation for result log
        {
            return $"{status,-9} {name} (id: {Id}, {age} years old)";
        }

        public override string ToString()
        {
            return $"{status,-9} {name} {Math.Max(serviceTime, 0),-2} {coordinate} {age}";
        }
    }
}
