A **.NET** project that starts with **Individual** accounts.

The objective is to take the cookie cutter project from .NET and make the following changes:

1. Implement **IdentityRole**
2. Implement custom models for Identity entities (see the Models folder)
3. Implement custom table names and properties in the ApplicationDbContext.cs.
4. Implement a custom **IUserClaimsPrincipalFactory<IdentityUser>**

# Steps to Implement the custom Identity entities
## Manually Create Models
The following custom models were manually created.  There is nothing magical about the names used.  But there is one big caveat.  When these classes are injected into ApplicationDbContext (see below), the TKey type for all of them must be the same and they must match TKey in the ApplicationDbContext implementation.

| Class | Implements |
|--|--|
| AppRole.cs | IdentityRole<int> |
| AppUser.cs | IdentityUser<int> |
| RoleClaim.cs | IdentityRoleClaim<in> |
| UserClaim.cs | IdentityUserClaim<int> |
| UserLogin.cs | IdentityUserLogin<int> |
| UserRole.cs | IdentityUserRole<int> |
| UserToken.cs | IdentityUserToken<int> |


## Inject Models in ApplicationDbContext

```
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int,
        UserClaim<AppUser>, UserRole<AppUser, AppRole>, UserLogin<AppUser>,
        RoleClaim<AppRole>, UserToken<AppUser>>

```
## Customize OnModelCreating
At this point, you can go ahead and run Add--Migration for EntityFrameworks.  And it will work.  But you will find that the table names and columns are going to be 100% identitical to just using IdentityUser without custom models.  So in order to have more control of the models, we manually edited OnModelCreating.  

```
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int,
        UserClaim<AppUser>, UserRole<AppUser, AppRole>, UserLogin<AppUser>,
        RoleClaim<AppRole>, UserToken<AppUser>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            
        // See ApplicationDbContext.cs for details.

```

How did we know what can be done in here?  We got started by copying the code straight out of https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0.

And at this point, if you want to use migrations to create the database, you can use 
```
PM> Add-Migration IdentitySchema
```
And that's how we ended up with the migrations added to the Migrations folder.


## Modifying the Service Injection in Program.cs
**AddDefaultIdentity** which is part of the .NET project template does not accept **IdentityRole**.  

So to implement **IdentityRole**, 

Edit **Program.cs** so the instead of...

```
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```
We use this...

```
builder.Services.AddIdentity<AppUser, AppRole>(options => { options.SignIn.RequireConfirmedEmail = false; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


```


# Alternative to AddDefaultUI()
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








