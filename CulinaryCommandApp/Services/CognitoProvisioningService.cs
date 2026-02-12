using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace CulinaryCommandApp.Services;

public class CognitoProvisioningService
{
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly string _userPoolId;

    public CognitoProvisioningService(IAmazonCognitoIdentityProvider cognito, IConfiguration config)
    {
        _cognito = cognito ?? throw new ArgumentNullException(nameof(cognito));
        _userPoolId = config["Authentication:Cognito:UserPoolId"]
            ?? throw new InvalidOperationException("Missing config: Authentication:Cognito:UserPoolId");
    }

    public async Task CreateUserWithPasswordAsync(string email, string displayName, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.", nameof(password));

        var username = email.Trim().ToLowerInvariant();
        var name = string.IsNullOrWhiteSpace(displayName) ? username : displayName.Trim();

        try
        {
            await _cognito.AdminCreateUserAsync(new AdminCreateUserRequest
            {
                UserPoolId = _userPoolId,
                Username = username,
                MessageAction = "SUPPRESS",
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "email", Value = username },
                    new AttributeType { Name = "email_verified", Value = "true" },
                    new AttributeType { Name = "name", Value = name }
                }
            });
        }
        catch (UsernameExistsException ex)
        {
            throw new InvalidOperationException($"A Cognito user with email '{username}' already exists.", ex);
        }
        catch (InvalidPasswordException ex)
        {
            throw new InvalidOperationException("Password does not meet the Cognito password policy.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create Cognito user.", ex);
        }

        try
        {
            await _cognito.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest
            {
                UserPoolId = _userPoolId,
                Username = username,
                Password = password,
                Permanent = true
            });
        }
        catch (InvalidPasswordException ex)
        {
            throw new InvalidOperationException("Password does not meet the Cognito password policy.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Cognito user was created, but setting the password failed.", ex);
        }
    }
}
