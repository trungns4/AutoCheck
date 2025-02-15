param (
    [string]$projectDir  # Accept the project directory as a parameter
)

# Construct the path to AssemblyInfo.cs
$assemblyInfoPath = "$projectDir\Properties\AssemblyInfo.cs"

# Debugging: Print the path for verification
Write-Host "Updating AssemblyInfo.cs at: $assemblyInfoPath"

# Ensure the file exists
if (!(Test-Path $assemblyInfoPath)) {
    Write-Host "Error: AssemblyInfo.cs not found at $assemblyInfoPath" -ForegroundColor Red
    exit 1
}

# Read the contents of AssemblyInfo.cs
$content = Get-Content $assemblyInfoPath

# Define versioning format (Major.Minor.Build.Revision)
$major = 1
$minor = 0
$build = (Get-Date).Year
$revision = (Get-Date).DayOfYear * 100 + (Get-Date).Hour * 10 + [math]::Floor((Get-Date).Minute / 10)

# Construct the new version number
$newVersion = "$major.$minor.$build.$revision"

# Replace the AssemblyVersion and AssemblyFileVersion lines
$content = $content -replace 'AssemblyVersion\(".*"\)', "AssemblyVersion(`"$newVersion`")"
$content = $content -replace 'AssemblyFileVersion\(".*"\)', "AssemblyFileVersion(`"$newVersion`")"

# Write back to AssemblyInfo.cs
$content | Set-Content $assemblyInfoPath

Write-Host "Updated version to: $newVersion"
