using AutoMapper;
using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;
using POS.CustomerRegistration.API.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace POS.CustomerRegistration.API.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly IIdentityService _identityService;
        private readonly ICustomerService _customerService;
        private readonly IGoogleService _googleService;
        private readonly IMapper _mapper;

        public CustomerRegistrationService(IIdentityService identityService, ICustomerService customerService, IGoogleService googleService, IMapper mapper)
        {
            _identityService = identityService;
            _customerService = customerService;
            _googleService = googleService;
            _mapper = mapper;
        }

        public async Task RegisterCustomerAsync(CustomerUserDto customerUserDto)
        {
            // Step 1: Register user identity
            Models.IdentityResult identityResult = await _identityService.RegisterIdentityAsync(_mapper.Map<RegisterModel>(customerUserDto));
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


        public async Task<LoginResult> GoogleAuthAsync([FromBody] GoogleLoginModel model)
        {
            // Retrieve user info from Google based on token
            GoogleUserInfo googleUserInfo = await _googleService.GetUserInfoAsync(model.Token);

            // validate user info
            if (googleUserInfo == null)
            {
                return new LoginResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to retrieve user info from Google"
                };
            }
            // Since google worked, gather IdentityAPI Token if Identity exists
            LoginResult loginResult = await _identityService.GoogleLoginAsync(model);

            if (!loginResult.IsSuccess) // if Login attempt against Identity Service failed means Identity for that email does not exist
            {
                Models.IdentityResult identityResult = await _identityService.RegisterIdentityAsync(new RegisterModel()
                                                                                                            {
                                                                                                                Email = googleUserInfo.Email,
                                                                                                                FirstName = googleUserInfo.GivenName,
                                                                                                                LastName = googleUserInfo.FamilyName
                                                                                                            } );

                if (!identityResult.IsSuccess) // if identity creation failed, return error
                {
                    loginResult.IsSuccess = false;
                    loginResult.ErrorMessage = "Failed to register user identity";
                    return loginResult;
                }
            }            

            // Try to retrieve customer info from CustomerAPI
            CustomerDto customerDto = await _customerService.GetCustomerByEmailAsync(googleUserInfo.Email, loginResult.Token);

            // validate we got customer info from CustomerAPI
            if (customerDto == null) //if we did not find a customer, create one
            {                
                // Create customer record with JWT token
                CustomerResult customerResult = await _customerService.CreateCustomerAsync(new CustomerDto
                                                                                                    {
                                                                                                        Email = googleUserInfo.Email,
                                                                                                        FirstName = googleUserInfo.GivenName,
                                                                                                        LastName = googleUserInfo.FamilyName
                                                                                                    }, loginResult.Token);

                if (!customerResult.IsSuccess)
                {                    
                    loginResult.IsSuccess = false;
                    loginResult.ErrorMessage = "Failed to create customer";
                    return loginResult;
                }
            }

            return loginResult;
        }
    }
}
