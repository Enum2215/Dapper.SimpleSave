﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Mothership.Core.Common.Dto
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        
        public long RoleId { get; set; }
    }
}
