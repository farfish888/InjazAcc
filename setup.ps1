# يجهّز الحل ويضيف المشاريع للسولوشن (تشغيله من مجلد InjazAcc)
dotnet new sln -n InjazAcc
dotnet sln add .\src\InjazAcc.UI\InjazAcc.UI.csproj
dotnet sln add .\src\InjazAcc.Domain\InjazAcc.Domain.csproj
dotnet sln add .\src\InjazAcc.Infrastructure\InjazAcc.Infrastructure.csproj
dotnet sln add .\src\InjazAcc.Services\InjazAcc.Services.csproj
dotnet sln add .\src\InjazAcc.Shared\InjazAcc.Shared.csproj
Write-Host "Solution created. Use: dotnet restore; dotnet run --project src/InjazAcc.UI/InjazAcc.UI.csproj"
