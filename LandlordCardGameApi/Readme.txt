Azure Subscription: https://portal.azure.com/#@kenkwok20hotmail.onmicrosoft.com/resource/subscriptions/31213f8b-9692-49c1-b0ee-365237a15917/overview
Resource Group: LandlordGame
API Web App: LandlordCardGameApi
Key Vault: landlordgamekv
Storage Account: landlordgamestorage
Communication Service: LandlordACS

Deployment steps:
In VS, right click on LandlordCardGameApi solution, and select publish
Pick the Azure information above and Target Framework net8.0

After publish, navigate to https://landlordcardgameapi-d4fkbke4ewdjbqcw.canadacentral-01.azurewebsites.net/swagger/index.html