using System;
using System.Collections.Generic;
using System.Linq;

namespace Task13
{
    internal class Cashier   // cash operations post model for a shop
    {
        int number;
        double coordinate;
        PersonStatus serveStatus = PersonStatus.Ordinary; // if set to non-Ordinary, then only persons with this status are served
        bool closed = false;       // do not serve at the moment
        bool paused = false;       // do not enqueue new clients
        PriorityQueue<Person, PersonStatus> queuePersons;

        public double Coordinate {
            get => coordinate; 
            set => coordinate = value;
        }

        public PersonStatus ServeStatus
        { 
            get => serveStatus; 
            set => serveStatus = value;
        }

        public int Number
        {
            get => number;
            set => number = value;
        }

        public Cashier() : this(0, 0) { }

        public Cashier(int number, double coordinate)
        {
            queuePersons = new();
            this.coordinate = coordinate;
            this.number = number;
        }

        public bool IsEmpty() => PersonsInQueue() == 0;

        public bool IsClosed() => closed;
        public bool IsPaused() => paused;

        public void PauseOrResume() // pause or resume clients enqueueing
        {
            paused = !paused;
            Program.Message($"*** Cashier #{number} is " + (paused ? "paused" : "resumed") + "! ***", 
                ConsoleColor.Yellow);
        }

        public void Open()    // (re)open for clients (after closing, for example)
        {
            if (!closed) return;
            closed = false;
            Program.Message($"*** Cashier #{number} is opened! ***", ConsoleColor.Yellow);
        }
        
        public int PersonsInQueue() => queuePersons.Count;

        public Person Peek() => queuePersons.Peek();  // the person with the highest priority in the queue

        public void Enqueue(Person person)
        {
            queuePersons.Enqueue(person, person.Status);
        }

        public Person Dequeue() // remove and return the highest priority client in the queue
            // priorities are given in the enum PersonStatus, the highest priority has LOWEST value
        {
            return queuePersons.Dequeue();
        }

        public List<Person> Close() // set the closed attribute and clear the queue if it is not empty
                                    // return the list of persons in the cashier's queue before closing
        {
            List<Person> personsList = new();
            if (closed)
            {   // this situation should not happen but for such a case...
                Program.Message($"*** Cashier #{number} is already closed! ***", ConsoleColor.Red);
                return personsList;
            }
            if (queuePersons is not null) 
                while (queuePersons.Count > 0)
                {
                    Person person = queuePersons.Dequeue();
                    personsList.Add(person);
                }
            closed = true;
            Program.Message($"*** Cashier #{number} is closed! ***", ConsoleColor.Yellow);
            return personsList;
        }

        public List<Person> DequeueOtherPersons(PersonStatus statusToServe)
            // dequeue persons with the status different from statusToServe and return their list
        {
            List<Person> personsWithThisStatus = new();
            List<Person> personsWithOtherStatus = new();
            if (queuePersons is not null)
                while (queuePersons.Count > 0)
                {
                    Person person = queuePersons.Dequeue();
                    if (person.Status == statusToServe)
                        personsWithThisStatus.Add(person);
                    else
                        personsWithOtherStatus.Add(person);
                }
            foreach (Person person in personsWithThisStatus)
                Enqueue(person); // put back to this cashier all persons with this status, others will be distributed to other cashiers
            return personsWithOtherStatus;
        }
    
        public void MoveStatusPersonsToCashier(Cashier cashier, PersonStatus statusToServe)
        {
            List<Person> personsWithThisStatus = new();
            List<Person> personsWithOtherStatus = new();
            if (queuePersons is not null)
                while (queuePersons.Count > 0)
                {
                    Person person = queuePersons.Dequeue();
                    if (person.Status == statusToServe)
                        personsWithThisStatus.Add(person);
                    else
                        personsWithOtherStatus.Add(person);
                }
            foreach (Person person in personsWithThisStatus)
            {
                cashier.Enqueue(person); // put to the given cashier all persons with this status
                Program.Message(
                    $"*** Person {person} is moved to Cashier #{cashier.Number} " +
                    $"(# {cashier.PersonsInQueue()} in queue)", ConsoleColor.Yellow);
            }
            foreach (Person person in personsWithOtherStatus)
                Enqueue(person);         // other persons put back to this cashier
        }
    }
}
