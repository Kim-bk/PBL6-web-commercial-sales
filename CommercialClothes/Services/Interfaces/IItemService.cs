using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services.Interfaces
{
    public interface IItemService
    {
        List<Item> GetItem();
    }
}