dotnet publish -c Release --framework net8.0
dotnet publish -c Release --framework net9.0
dotnet publish -c Release --framework netstandard2.0
dotnet pack --include-symbols --include-source