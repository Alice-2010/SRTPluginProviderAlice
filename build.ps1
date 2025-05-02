if ($args.Count -lt 2) {
	Write-Host "Usage: build.ps1 <Platform> <Configuration>"
	exit 1
}

$platform = $args[0]
if ($platform -ne "x64" -and $platform -ne "x86") {
	Write-Host "Platform must be x64 or x86. Received $platform"
	exit 1
}

$conf = $args[1]
if ($conf -ne "Debug" -and $conf -ne "Release") {
	Write-Host "Configuration must be Debug or Release. Received $conf"
	exit 1
}

dotnet build SRTPluginProviderAlice.sln -c "$conf" /p:Platform="$platform"

if (Test-Path -Path build) {
	Remove-Item -Path build -Recurse -Force
}

New-Item -ItemType "Directory" -Path build
New-Item -ItemType "Directory" -Path build/ref

Copy-Item -Path "bin/$platform/$conf/netstandard2.1/ProcessMemory32.dll" -Destination "build/ProcessMemory32.dll"
Copy-Item -Path "bin/$platform/$conf/netstandard2.1/SRTPluginProviderAlice.dll" -Destination "build/SRTPluginProviderAlice.dll"
Copy-Item -Path "bin/$platform/$conf/netstandard2.1/SRTPluginProviderAlice.deps.json" -Destination "build/SRTPluginProviderAlice.deps.json"
Copy-Item -Path "obj/$platform/$conf/netstandard2.1/SRTPluginProviderAlice.dll" -Destination "build/ref/SRTPluginProviderAlice.dll"

Write-Host "Build for $platform $conf succeeded. Files are present in the 'build' folder"