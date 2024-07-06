namespace VetVaxManager.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char? Sex { get; set; }
        public string? Race { get; set; }
        public decimal? Weight { get; set; }
        public bool Alive { get; set; }
        public Specie? Specie { get; set; }
        public Owner? Owner { get; set; }
    }
}
