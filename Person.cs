using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons_with_WPF
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsMale { get; set; }
        public bool HasDriversLicence { get; set; }

        public Person(string firstName, string lastName, DateTime birthdate, bool isMale, bool hasDriversLicence)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
            IsMale = isMale;
            HasDriversLicence = hasDriversLicence;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} - {Birthdate} [{(IsMale ? "Male" : "Female")} {(HasDriversLicence ? ",Driver" : "")}]";
        }

        public string ToCsvString()
        {
            return $"{FirstName};{LastName};{IsMale};{HasDriversLicence};{Birthdate}";
        }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   FirstName == person.FirstName &&
                   LastName == person.LastName &&
                   Birthdate == person.Birthdate &&
                   IsMale == person.IsMale &&
                   HasDriversLicence == person.HasDriversLicence;
        }

        public static Person Parse(string line)
        {
            String[] split = line.Split(";");
            return new Person(split[0], split[1], Convert.ToDateTime(split[4]), Convert.ToBoolean(split[2]), Convert.ToBoolean(split[3]));
        }

        public static bool TryParse(string line, Person expected)
        {
            return Parse(line).Equals(expected);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, Birthdate, IsMale, HasDriversLicence);
        }
    }
}
