param(
    [string]$AwsAccountId = "",

    [string]$AwsRegion = "ap-northeast-2",
    [string]$RepositoryName = "ba-chatbot-api",
    [string]$AppVersion = "",
    [string]$ImageTag = "",
    [switch]$PushLatest,

    [string]$LightsailHost = "",
    [string]$LightsailUser = "ubuntu",
    [string]$RemoteDirectory = "~/ba-chatbot-api",
    [string]$ChatbotApiTokens = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDirectory
$versionPath = Join-Path $repoRoot "VERSION"
if ([string]::IsNullOrWhiteSpace($AppVersion)) {
    $AppVersion = if (Test-Path -LiteralPath $versionPath) {
        (Get-Content -LiteralPath $versionPath -Raw).Trim()
    }
    else {
        "0.0.0"
    }
}
if ($AppVersion -notmatch '^\d+\.\d+\.\d+(-[0-9A-Za-z][0-9A-Za-z.-]*)?$') {
    throw "AppVersion must be a NuGet/SemVer version such as 0.1.0 or 0.1.0-preview. Docker image tags are separate; use -ImageTag for values like 20260528_01."
}
if ([string]::IsNullOrWhiteSpace($ImageTag)) {
    $ImageTag = "$AppVersion-$(Get-Date -Format "yyyyMMddHHmm")"
}

if ([string]::IsNullOrWhiteSpace($AwsAccountId)) {
    Write-Host "AwsAccountId was not supplied. Resolving from AWS CLI credentials..."
    $AwsAccountId = (& aws sts get-caller-identity --query Account --output text).Trim()
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($AwsAccountId)) {
        throw "Could not resolve AWS account id. Run 'aws configure' or pass -AwsAccountId with your 12-digit AWS account number."
    }
}
if ($AwsAccountId -match '^(AKIA|ASIA)[A-Z0-9]+$') {
    throw "AwsAccountId received an AWS access key id. Pass the 12-digit AWS account number instead, or omit -AwsAccountId to auto-detect it with AWS CLI."
}
if ($AwsAccountId -notmatch '^\d{12}$') {
    throw "AwsAccountId must be the 12-digit AWS account number, for example 123456789012. Current value: '$AwsAccountId'"
}

$registry = "$AwsAccountId.dkr.ecr.$AwsRegion.amazonaws.com"
$localImage = "${RepositoryName}:${ImageTag}"
$imageUri = "${registry}/${RepositoryName}:${ImageTag}"
$latestUri = "${registry}/${RepositoryName}:latest"
$buildDate = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
$gitSha = "unknown"
& git -C $repoRoot rev-parse --short HEAD *> $null
if ($LASTEXITCODE -eq 0) {
    $gitSha = (& git -C $repoRoot rev-parse --short HEAD).Trim()
}
$gitSha = $gitSha -replace '[^0-9A-Za-z.-]', '.'

function Invoke-Checked {
    param(
        [Parameter(Mandatory = $true)]
        [string]$CommandPath,

        [Parameter(ValueFromRemainingArguments = $true)]
        [string[]]$Arguments
    )

    & $CommandPath @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Command failed: $CommandPath $($Arguments -join ' ')"
    }
}

Push-Location $repoRoot
try {
    Write-Host "Building solution..."
    Invoke-Checked dotnet build "AIChatBotService.slnx" "--no-restore"

    Write-Host "Ensuring ECR repository exists: $RepositoryName"
    & aws ecr describe-repositories --repository-names $RepositoryName --region $AwsRegion *> $null
    if ($LASTEXITCODE -ne 0) {
        Invoke-Checked aws ecr create-repository --repository-name $RepositoryName --region $AwsRegion
    }

    Write-Host "Logging in to ECR: $registry"
    & aws ecr get-login-password --region $AwsRegion | & docker login --username AWS --password-stdin $registry
    if ($LASTEXITCODE -ne 0) {
        throw "ECR docker login failed."
    }

    Write-Host "Building Docker image: $localImage"
    Write-Host "App version: $AppVersion"
    Write-Host "Build date: $buildDate"
    Write-Host "Git SHA: $gitSha"
    $dockerBuildArgs = @(
        "build",
        "--build-arg", "APP_VERSION=$AppVersion",
        "--build-arg", "IMAGE_TAG=$ImageTag",
        "--build-arg", "BUILD_DATE=$buildDate",
        "--build-arg", "GIT_SHA=$gitSha",
        "-f", "deploy/Dockerfile",
        "-t", $localImage,
        "."
    )
    Invoke-Checked docker @dockerBuildArgs

    Write-Host "Tagging Docker image: $imageUri"
    Invoke-Checked docker tag $localImage $imageUri

    if ($PushLatest) {
        Write-Host "Tagging Docker image: $latestUri"
        Invoke-Checked docker tag $localImage $latestUri
    }

    Write-Host "Pushing Docker image: $imageUri"
    Invoke-Checked docker push $imageUri

    if ($PushLatest) {
        Write-Host "Pushing Docker image: $latestUri"
        Invoke-Checked docker push $latestUri
    }

    if ([string]::IsNullOrWhiteSpace($LightsailHost)) {
        Write-Warning "Image was pushed to ECR only. LightsailHost was not supplied, so the running Lightsail container was not updated."
        Write-Host "Image URI: $imageUri"
        Write-Host ""
        Write-Host "To update Lightsail, rerun with -LightsailHost, for example:"
        Write-Host ".\deploy\publish-ecr-lightsail.ps1 -AwsAccountId `"$AwsAccountId`" -ImageTag `"$ImageTag`" -LightsailHost `"your-lightsail-public-ip`""
        Write-Host ""
        Write-Host "Or run these commands on the Lightsail server after updating docker-compose.yml to use the image above:"
        Write-Host "aws ecr get-login-password --region $AwsRegion | docker login --username AWS --password-stdin $registry"
        Write-Host "cd $RemoteDirectory"
        Write-Host "grep 'image:' docker-compose.yml"
        Write-Host "docker compose -p ba-chatbot-api pull"
        Write-Host "docker compose -p ba-chatbot-api up -d --force-recreate"
        Write-Host "docker inspect ba-chatbot-api --format '{{.Config.Image}}'"
        return
    }

    $target = "${LightsailUser}@${LightsailHost}"
    $tempDirectory = Join-Path ([System.IO.Path]::GetTempPath()) "ba-chatbot-deploy"
    New-Item -ItemType Directory -Force -Path $tempDirectory | Out-Null

    $composePath = Join-Path $tempDirectory "docker-compose.yml"
    $compose = @'
services:
  chatbot-api:
    image: __IMAGE_URI__
    container_name: ba-chatbot-api
    restart: unless-stopped
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Chatbot__KbPath: /app/ChatBot/ba_manual_vector.db
      Chatbot__ModelPath: ""
      Chatbot__TopK: "6"
      Chatbot__MinScore: "0"
      CHATBOT_API_TOKENS: ${CHATBOT_API_TOKENS}
'@
    $compose.Replace("__IMAGE_URI__", $imageUri) | Set-Content -LiteralPath $composePath -Encoding UTF8

    Write-Host "Preparing Lightsail directory: $RemoteDirectory"
    Invoke-Checked ssh $target "mkdir -p $RemoteDirectory"
    Invoke-Checked scp $composePath "${target}:${RemoteDirectory}/docker-compose.yml"

    if (-not [string]::IsNullOrWhiteSpace($ChatbotApiTokens)) {
        $envPath = Join-Path $tempDirectory ".env"
        "CHATBOT_API_TOKENS=$ChatbotApiTokens" | Set-Content -LiteralPath $envPath -Encoding UTF8
        Invoke-Checked scp $envPath "${target}:${RemoteDirectory}/.env"
    }

    $remoteCommand = "cd $RemoteDirectory && grep 'image:' docker-compose.yml && aws ecr get-login-password --region $AwsRegion | docker login --username AWS --password-stdin $registry && docker compose -p ba-chatbot-api pull && docker compose -p ba-chatbot-api up -d --force-recreate && docker compose -p ba-chatbot-api ps && docker inspect ba-chatbot-api --format '{{.Config.Image}}'"
    Write-Host "Deploying on Lightsail: $target"
    Invoke-Checked ssh $target $remoteCommand

    Write-Host "Deployment completed."
    Write-Host "App version: $AppVersion"
    Write-Host "Image URI: $imageUri"
}
finally {
    Pop-Location
}
