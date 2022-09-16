﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DAL.Repositories
{
    public class UserRepository : Repository<Account>, IUserRepository
    {
        public UserRepository(DbFactory dbFactory) : base(dbFactory)
        { 
        }
    }
}