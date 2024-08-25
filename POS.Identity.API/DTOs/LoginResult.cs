﻿namespace POS.Identity.API.DTOs
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
