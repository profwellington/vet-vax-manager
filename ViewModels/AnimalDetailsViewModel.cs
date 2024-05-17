using VetVaxManager.Models;

namespace VetVaxManager.ViewModels
{
    public class AnimalDetailsViewModel
    {
        public Animal Animal { get; set; }
        public IList<Calendar> Calendar { get; set; }
        public IList<Vaccine> Vaccines { get; set; }
    }
}
