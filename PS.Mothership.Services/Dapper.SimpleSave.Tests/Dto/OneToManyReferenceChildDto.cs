﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.SimpleSave.Tests.Dto {
    [Table("dbo.OneToManyReferenceChild")]
    [ReferenceData]
    public class OneToManyReferenceChildDto : BaseOneToManyChildDto {
    }
}
