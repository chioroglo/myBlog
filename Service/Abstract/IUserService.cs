﻿using Domain;

namespace Service.Abstract
{
    public interface IUserService : IEntityService<User>
    {
        Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        Task UpdateLastActivity(int userId, CancellationToken cancellationToken);
    }
}