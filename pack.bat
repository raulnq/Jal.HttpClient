packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.HttpClient\Jal.HttpClient.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.HttpClient.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.HttpClient.Installer\Jal.HttpClient.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.HttpClient.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.HttpClient.Logger\Jal.HttpClient.Logger.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.HttpClient.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.HttpClient.Logger.Installer\Jal.HttpClient.Logger.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.HttpClient.Nuget

pause;