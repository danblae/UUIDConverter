Push-Location
Set-Location $PSScriptRoot

$name = 'UUIDConverter'
$assembly = "Community.PowerToys.Run.Plugin.$name"
$version = "v$((Get-Content ./plugin.json | ConvertFrom-Json).Version)"
$archs = @('x64', 'arm64')
$tempDir = "./out/$name"

git tag $version
git push --tags

Remove-Item ./out/*.zip -Recurse -Force -ErrorAction Ignore
foreach ($arch in $archs) {
	$releasePath = "./bin/$arch/Release/net9.0-windows10.0.17763.0"

	dotnet build -c Release /p:Platform=$arch

	Remove-Item "$tempDir/*" -Recurse -Force -ErrorAction Ignore
	mkdir "$tempDir" -ErrorAction Ignore
	$items = @(
		"$releasePath/$assembly.deps.json",
		"$releasePath/$assembly.dll",
		"$releasePath/plugin.json",
		"$releasePath/Images"
	)
	Copy-Item $items "$tempDir" -Recurse -Force
	Compress-Archive "$tempDir" "./out/$name-$version-$arch.zip" -Force
}

gh release create $version (Get-ChildItem ./out/*.zip)
Pop-Location
