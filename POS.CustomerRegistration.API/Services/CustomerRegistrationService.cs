﻿using AutoMapper;
using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;
using POS.CustomerRegistration.API.IServices;

namespace POS.CustomerRegistration.API.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly IIdentityService _identityService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerRegistrationService(IIdentityService identityService, ICustomerService customerService, IMapper mapper)
        {
            _identityService = identityService;
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task RegisterCustomerAsync(CustomerUserDto customerUserDto)
        {
            // Step 1: Register user identity
            IdentityResult identityResult = await _identityService.RegisterIdentityAsync(_mapper.Map<RegisterModel>(customerUserDto));
            if (!identityResult.IsSuccess)
            {
                throw new Exception("Failed to register user identity");
            }

            // Step 2: Login to get JWT token
            var loginResult = await _identityService.LoginAsync(new LoginModel
            {
                Email = customerUserDto.Email, 
                Password = customerUserDto.Password
            });

            if (!loginResult.IsSuccess)
            {
                // Rollback identity creation if login fails
                await _identityService.DeleteIdentityAsync(identityResult.UserId);
                throw new Exception("Failed to login user");
            }

            // Step 3: Create customer record with JWT token
            var customerResult = await _customerService.CreateCustomerAsync(_mapper.Map<CustomerDto>(customerUserDto), loginResult.Token);
            if (!customerResult.IsSuccess)
            {
                // Rollback identity creation if customer creation fails
                await _identityService.DeleteIdentityAsync(identityResult.UserId);
                throw new Exception("Failed to create customer");
            }
        }
    }
}
