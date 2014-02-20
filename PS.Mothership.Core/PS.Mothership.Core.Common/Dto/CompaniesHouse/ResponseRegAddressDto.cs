﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PS.Mothership.Core.Common.Dto.CompaniesHouse
{
    [CollectionDataContract(ItemName = "AddressLine", Namespace = "http://xmlgw.companieshouse.gov.uk/v1-0/schema")]
    public class ResponseRegAddressDto : List<string> { }
}
