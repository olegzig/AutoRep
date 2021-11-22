namespace AutoRep.Models
{
    public class User
    {
        public User()
        { }

        public int Id { get; set; }
        public string Name { get; set; }//ФИО работника/гово
        public bool IsOwner { get; set; }//t = владелец, f = работник

        public override string ToString()
        {
            return Id.ToString() + " " + Name + " " + IsOwner.ToString();
        }

        public enum SortState
        {
            NameAsc,
            NameDesc
        }
    }
}