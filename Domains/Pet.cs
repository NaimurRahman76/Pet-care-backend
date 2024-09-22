namespace PetCareBackend.Domains
{
    public class Pet : BaseModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }

    }
}
