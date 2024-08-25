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
            // Register user identity
            IdentityResult identityResult = await _identityService.RegisterIdentityAsync(_mapper.Map<RegisterModel>(customerUserDto));
            if (!identityResult.IsSuccess) // if identity registration failed
            {
                // Check for duplicate user error
                if (identityResult.ErrorMessage == "User with this email already exists.")
                {
                    //try to validate credentials to see if the one trying to register is the same Identity as the one that already exists
                    var loginResult = await _identityService.LoginAsync(new LoginModel
                    {
                        Email = customerUserDto.Email,
                        Password = customerUserDto.Password
                    });

                    // if login is not successful, maybe somebody else trying to use an email that already exists. return the error
                    if (!loginResult.IsSuccess)
                    {
                        return new LoginResult
                        {
                            IsSuccess = false,
                            ErrorMessage = identityResult.ErrorMessage
                        };
                    }

                    // if login is successful, it means the user trying to register is the same as the Identity
                    identityResult.IsSuccess = loginResult.IsSuccess;
                    identityResult.Token = loginResult.Token;

                    // check if customer exists for this email
                    var customerExists = await _customerService.GetCustomerByEmailAsync(customerUserDto.Email, loginResult.Token);
                    
                    if (customerExists != null) //if customer exists, given it's the same as the one trying to register, just return a valid login  result
                    {
                        return new LoginResult
                        {
                            IsSuccess = true,
                            Token = loginResult.Token,
                            Customer = customerExists
                        };
                    }// if customer does not exist, continue to create a new customer record since it's a valid login                    
                }
            }

            if (!identityResult.IsSuccess)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create customer. " + identityResult.ErrorMessage
                };
            }

            // Create customer record with JWT token
            var customerResult = await _customerService.CreateCustomerAsync(_mapper.Map<CustomerDto>(customerUserDto), identityResult.Token);
            if (!customerResult.IsSuccess)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to create customer"
                };
            }

            // Retrieve the newly created customer info
            var customerDto = await _customerService.GetCustomerByEmailAsync(customerUserDto.Email, identityResult.Token);

            return new LoginResult
            {
                IsSuccess = true,
                Token = identityResult.Token,
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
