﻿using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface IAvatarRepository : IBaseRepository<Avatar>
    {
        Task<Avatar?> GetByUserIdAsync(int userId);
    }
}
