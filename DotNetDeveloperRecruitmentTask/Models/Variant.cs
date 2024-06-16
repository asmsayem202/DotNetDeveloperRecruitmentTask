namespace DotNetDeveloperRecruitmentTask.Models
{
    public enum Size
    {
        Small,
        Medium,
        Large
        
    }


    public class Variant
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Specification { get; set; }
        public Size Size { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        

    }

}

