namespace VetVaxManager.Models
{
    public class Owner
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Sex { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
