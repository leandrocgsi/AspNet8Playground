﻿using RestWithASPNETUdemy.Models;

namespace RestWithASPNETUdemy.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(User user);

        User ValidateCredentials(string username);

        User RefreshUserInfo(User user);

        bool RevokeToken(string username);
    }
}
