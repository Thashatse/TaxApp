﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ClientDetailsViewModel
    {
        public Client Client { get; set; }

        public List<Model.SP_GetJob_Result> Jobs { get; set; }
        public List<Model.SP_GetInvoice_Result> Invoices { get; set; }
    }
}
