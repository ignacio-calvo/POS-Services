using AutoMapper;
using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;
using POS.CustomerIdentity.API.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace POS.CustomerIdentity.API.Services
{
    public class CustomerIdentityService : ICustomerIdentityService
    {
        private readonly IIdentityService _identityService;
        private readonly ICustomerService _customerService;
        private readonly IGoogleService _googleService;
        private readonly IMapper _mapper;

        public CustomerIdentityService(IIdentityService identityService, ICustomerService customerService, IGoogleService googleService, IMapper mapper)
        {
            _identityService = identityService;
            _customerService = customerService;
            _googleService = googleService;
            _mapper = mapper;
        }

        public async Task<LoginResult> RegisterCustomerAsync(CustomerUserDto customerUserDto)
        {
            // Step 1: Register user identity
            Models.IdentityResult identityResult = await _identityService.RegisterIdentityAsync(_mapper.Map<RegisterModel>(customerUserDto));
            if (!identityResult.IsSuccess)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to register user identity"
                };
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
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to login user"
                };
            }

            // Step 3: Create customer record with JWT token
            var customerResult = await _customerService.CreateCustomerAsync(_mapper.Map<CustomerDto>(customerUserDto), loginResult.Token);
            if (!customerResult.IsSuccess)
            {
                // Rollback identity creation if customer creation fails
                await _identityService.DeleteIdentityAsync(identityResult.UserId);
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create customer"
                };
            }

            // Retrieve the newly created customer info
            var customerDto = await _customerService.GetCustomerByEmailAsync(customerUserDto.Email, loginResult.Token);

            return new LoginResult
            {
                IsSuccess = true,
                Token = loginResult.Token,
                Customer = customerDto
            };
        }

        public async Task<LoginResult> GoogleAuthAsync([FromBody] GoogleLoginModel model)
        {
            // Retrieve user info from Google based on token
            GoogleUserInfo googleUserInfo = await _googleService.GetUserInfoAsync(model.Token);

            // Validate user info
            if (googleUserInfo == null)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to retrieve user info from Google"
                };
            }

            // Since Google worked, gather IdentityAPI Token if Identity exists
            LoginResult loginResult = await _identityService.GoogleLoginAsync(model);

            if (!loginResult.IsSuccess) // if Login attempt against Identity Service failed means Identity for that email does not exist
            {
                Models.IdentityResult identityResult = await _identityService.RegisterIdentityAsync(new RegisterModel
                {
                    Email = googleUserInfo.Email,
                    FirstName = googleUserInfo.GivenName,
                    LastName = googleUserInfo.FamilyName
                });

                if (!identityResult.IsSuccess) // if identity creation failed, return error
                {
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Failed to register user identity"
                    };
                }

                // Re-attempt login after registration
                loginResult = await _identityService.GoogleLoginAsync(model);
                if (!loginResult.IsSuccess)
                {
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Failed to login user after registration"
                    };
                }
            }

            // Try to retrieve customer info from CustomerAPI
            CustomerDto customerDto = await _customerService.GetCustomerByEmailAsync(googleUserInfo.Email, loginResult.Token);

            // Validate we got customer info from CustomerAPI
            if (customerDto == null) // if we did not find a customer, create one
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
                    return new LoginResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Failed to create customer"
                    };
                }

                // Retrieve the newly created customer info
                customerDto = await _customerService.GetCustomerByEmailAsync(googleUserInfo.Email, loginResult.Token);
            }

            // Include the customer info in the login result
            loginResult.Customer = customerDto;

            return loginResult;
        }

        public async Task<LoginResult> LoginAsync(LoginModel model)
        {
            // Use the identity service to perform the login
            var loginResult = await _identityService.LoginAsync(model);

            if (!loginResult.IsSuccess)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid login credentials"
                };
            }

            // Retrieve customer information
            var customerDto = await _customerService.GetCustomerByEmailAsync(model.Email, loginResult.Token);

            if (customerDto == null)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Customer not found"
                };
            }

            return new LoginResult
            {
                IsSuccess = true,
                Token = loginResult.Token,
                Customer = customerDto
            };
        }
    }
}
