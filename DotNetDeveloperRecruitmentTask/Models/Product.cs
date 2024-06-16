namespace DotNetDeveloperRecruitmentTask.Models
{

    public enum ProductType
    {
        Mug,
        Jug,
        Cup,
        Glass,
        Plate
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public ProductType Type { get; set; }
        public List<Variant> Variants { get; set; } = new List<Variant>();

       
    }
}
