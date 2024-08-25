﻿namespace POS.CustomerIdentity.API.Models
{
    public class IdentityResult
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
