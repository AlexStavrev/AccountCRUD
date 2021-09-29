namespace DBExerciseClassLibrary
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Account(int id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        public Account()
        {

        }

        public override string ToString()
        {
            return $"Account {Id}). {Name}, ${Balance};";
        }
    }
}
