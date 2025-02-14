using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Models
{
    public interface IEventsDto
    {
        int ID_EVENT { get; set; }
        int ID_COMPANY { get; set; }
        string DESC_EVENT_SP { get; set; }
        string DESC_EVENT_EN { get; set; }
        byte ACTIVE { get; set; }
        DateTime CREATED_DATE { get; set; }
        int CREATED_BY { get; set; }
        int MODIFY_DATE { get; set; }
        DateTime MODIFY_BY { get; set; }
    }
}
