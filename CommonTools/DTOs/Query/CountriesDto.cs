namespace CommonTools.DTOs.Query
{
    public class CountriesDto
    {

        public int ID_COUNTRY { get; set; }
        public string DESC_COUNTRY_SP { get; set; }
        public string DESC_COUNTRY_EN { get; set; }
        public bool RESTRICTION { get; set; }
        public byte ACTIVE { get; set; }
    }
}
