namespace VmCommanding.Model
{
    public class Person
    {
        public static Person[] GetPeople()
        {
            return new Person[]
            {
                new Person("Barney", 30),
                new Person("Fred", 32),
                new Person("Wilma", 28)
            };
        }

        private Person(string name, int age)
        {
            Name = name;
            Age = age;
            IsAlive = true;
        }

        public int Age { get; private set; }
        public bool IsAlive { get; set; }
        public string Name { get; private set; }
    }
}
