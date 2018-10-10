# TCG-Shop
Hogeschool Project C Leerjaar 2, Trading Card Game Webshop


# To Enable Email Confirmation on Registration

It's important to set the user-secrets, otherwise the application cannot send email.

this can be done by running:

dotnet user-secrets set SendGridUser MyUsername
  
  
dotnet user-secrets set SendGridKey MySendGridAPIKey
