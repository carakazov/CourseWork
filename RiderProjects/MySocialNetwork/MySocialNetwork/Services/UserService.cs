﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Utils;

namespace MySocialNetwork.Services
{
    public class UserService
    {
        private Mapper mapper = new Mapper();
        private Cryptographer cryptographer = new Cryptographer();
        private UserManager userManager = new UserManager();
        private PageManager pageManager = new PageManager();

        public void RegistrateNewUser(RegistrationDto registrationDto)
        {
            string hashedPassword = Cryptographer.Hash(registrationDto.Password);
            registrationDto.Password = hashedPassword;
            User user = mapper.FromRegistrationDtoToUser(registrationDto);
            userManager.AddUser(user);
        }
        public UserDto LogIn(string login)
        {
            User user = userManager.GetUserByLogin(login);
            UserDto userDto = mapper.FromUserToUserDto(user);
            return userDto;
        }

        public void UpdateScoredPostList(UserDto userDto)
        {
            User user = userManager.GetUserByLogin(userDto.Login);
            userDto.ScoredPosts = mapper.FindScoredPosts(user);
        }

        public List<AuthorDto> FindUsers(FindUsersDto userInfo)
        {
            List<User> users = userManager.FindUsers(userInfo);
            List<AuthorDto> usersDtos = mapper.FromUsersToAuthorsDtos(users);
            return usersDtos;
        }
    }
}