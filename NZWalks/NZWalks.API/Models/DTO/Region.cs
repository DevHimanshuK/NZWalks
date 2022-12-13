namespace NZWalks.API.Models.DTO
{
    //Region DTO, not actual model class
    //Currently it looks same but DTO can have different properties than Domain
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
    }
}
