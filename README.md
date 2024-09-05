A **.NET** project that starts with **Individual** accounts.

The objective is to take the cookie cutter project from .NET and make the following changes:

1. Implement **IdentityRole**
2. Implement custom models for Identity entities (see the Models folder)
3. Implement custom table names and properties in the ApplicationDbContext.cs.
4. Implement a custom **IUserClaimsPrincipalFactory<IdentityUser>**

**AddDefaultIdentity** which is part of the .NET project template does not accept **IdentityRole**.  

So to implement **IdentityRole**, 

Edit **Program.cs** so the instead of...

```
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```
We use this...

```
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
```


## Alternative to AddDefaultUI()
The AddDefaultUI injection for AddIdentity (see Program.cs) provides a login page and controller action (not visible in the Solution Explorer) for logging in.  If you don't want to do this, you can create your own view for Login and your own HTTPGet and HTTPPost controller actions for login.

Some ideas to manually handle Login from the HTTPPost would be:

In the controller constructor you will need to inject the following:

```
public MyController(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager
)
```

In the login controller action

```
//This is an optional way to find the user record in the data store (like ApplicationDbContext) so the user record can be used for more details about the login
//or can be updated manually.
var user = await _userManager.FindByEmailAsync(model.Email);

// If login is successful, PasswordSignInAsync method will trigger
// the Task<ClaimsPrincipal> CreateAsync(IdentityUser user) method of UserClaimsPrincipalFactory<IdentityUser, IdentityRole> 
var loginresult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: user.LockoutEnabled);


if (loginresult.Succeeded)
{
    //Do some kind of redirect here which could be included in the post parameters or data

}


```








