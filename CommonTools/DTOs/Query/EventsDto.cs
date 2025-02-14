using System;

namespace CommonTools.DTOs.Query
{
    public class EventsDto
    {
        public int ID_EVENT { get; set; }
        public int ID_COMPANY { get; set; }
        public int ID_EVENT_TYPE { get; set; }
        public int? ID_ESTATE { get; set; }
        public string DESC_LOCATION { get; set; }
        public string DESC_EVENT_SP { get; set; }
        public string DESC_EVENT_EN { get; set; }
        public bool ACTIVE { get; set; }
        public string DATE_INI { get; set; }
        public string DATE_FIN { get; set; }
    }
}
