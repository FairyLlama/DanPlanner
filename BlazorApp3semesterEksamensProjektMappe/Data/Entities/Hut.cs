

namespace BlazorApp3semesterEksamensProjektMappe.Data.Entities
{
    public class Hut

    {
        public int Id { get; set; }

        public int MaxCapacity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }

}
