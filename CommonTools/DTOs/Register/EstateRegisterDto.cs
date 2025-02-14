using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class EstateRegisterDto
    {
        public int ID_ESTATE { get; set; }
        public string IM_ESTATE { get; set; }
        public string DESC_ESTATE_SP { get; set; }
        public string DESC_ESTATE_EN { get; set; }
        public string ARB_ESTATE { get; set; }
        public int CAPACITY_ESTATE { get; set; }
        public string TYPE_ESTATE { get; set; }
        public bool ACTIVE { get; set; } = true;
        public int CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int? MODIFY_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }

    }
}
