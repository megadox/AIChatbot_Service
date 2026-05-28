param(
    [string]$Configuration = "Debug",
    [string]$OutputDirectory = "qa/feature_test_results"
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$casesDirectory = Join-Path $PSScriptRoot "feature_tests"
$outputRoot = Join-Path $repoRoot $OutputDirectory

New-Item -ItemType Directory -Force -Path $outputRoot | Out-Null

dotnet build (Join-Path $repoRoot "src/Tools.RetrievalSmokeTest") --no-restore
if ($LASTEXITCODE -ne 0) {
    throw "Build failed."
}

$failed = 0
foreach ($caseFile in Get-ChildItem -Path $casesDirectory -Filter "*.json" | Sort-Object Name) {
    $outputFile = Join-Path $outputRoot ($caseFile.BaseName + "_result.json")
    dotnet run --project (Join-Path $repoRoot "src/Tools.RetrievalSmokeTest") --no-build -- --cases $caseFile.FullName --output $outputFile
    if ($LASTEXITCODE -ne 0) {
        throw "Feature test runner failed: $($caseFile.Name)"
    }

    $report = Get-Content -LiteralPath $outputFile -Raw | ConvertFrom-Json
    if ($report.Failed -gt 0) {
        $failed += $report.Failed
    }
}

if ($failed -gt 0) {
    throw "Feature tests failed: $failed"
}

Write-Host "Feature tests passed."
