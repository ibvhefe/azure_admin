# Run powershell script as admin

This derives from the azure admin project with one custom controller, which allows for executing files from the repository.

Azure yaml task example:
```
- task: PowerShell@2
  displayName: "Run powershell in admin mode"
  inputs:
    targetType: 'inline'
    script: |  
      $scriptPath = "$(System.DefaultWorkingDirectory)\scripts\SampleScript.ps1" #lies in repository
      $url = "http://localhost:5005/Admin/ExecutePowershellScript?path=$($scriptPath)"
      $pipeline = Invoke-RestMethod -Uri $url -Method Get;
      Write-Host "Pipeline = $($pipeline | ConvertTo-Json -Depth 100)"
```

# Installation guide from the original project

* Azure Admin
Access Elevated Privileged Operations with Azure Self-Hosted Pipelines

* Features
	* .Net Core Worker Service
	* Run powershell web requests from Azure yml file
	* Run service as appropriate admin to run elevated commands
    * USE CASE : Remove Appx Package before installation for UI testing

* Quick Publish

```
cd $proj
dotnet restore
dotnet publish -o $Path
```

* Install Service

- In elevated terminal!

```
sc.exe create AzureAdmin binpath= $Path\AzureAdmin.exe
sc.exe start AzureAdmin
```
