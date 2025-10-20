# 1) Publish the app
dotnet publish ./src/WeekDayWebApplication/WeekDayWebApplication.csproj -c Release -o ./publish

# 2) Create ZIP of the publish output
cd ./publish && zip -r ../weekday_publish.zip . && cd ..

# 3) Deploy ZIP to the App Service (uses your az login)
az webapp deployment source config-zip --resource-group WebAppService1 --name weekday --src weekday_publish.zip

# 4) Quick check the site 
curl -I https://weekday.azurewebsites.net

