using namespace System.Text.RegularExpressions
using namespace System.IO

param(
    [string]$Uri = $(throw 'Parameter Uri is required'),
    [string]$Jwt = $(throw 'Parameter Jwt is required'),
    [string]$ReleaseId = $(throw 'Paramater ReleaseId is required'),
    [string]$FilesPath = $(throw 'Paramater FilesPath is required')
)

if (![Directory]::Exists($FilesPath)) {
    Write-Error -Message "FilesPath=$FilesPath não existe"
    exit 1
}

$files = [Directory]::GetFiles($FilesPath)

foreach ($file in $files) {
    if (!$file.EndsWith(".exe")) {
        continue
    }

    Write-Output "Preparing to upload attach file: $file"

    try {
        $fileName = [Path]::GetFileName($file)
        $attachTitle = $fileName.Substring(0, $fileName.Length - 4)

        # Script body
        $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $headers.Add("Authorization", "Bearer $Jwt")

        $multipartContent = [System.Net.Http.MultipartFormDataContent]::new()
        $stringHeader = [System.Net.Http.Headers.ContentDispositionHeaderValue]::new("form-data")
        $stringHeader.Name = "id_release"
        $stringContent = [System.Net.Http.StringContent]::new($ReleaseId)
        $stringContent.Headers.ContentDisposition = $stringHeader
        $multipartContent.Add($stringContent)

        $stringHeader = [System.Net.Http.Headers.ContentDispositionHeaderValue]::new("form-data")
        $stringHeader.Name = "title"
        $stringContent = [System.Net.Http.StringContent]::new($attachTitle)
        $stringContent.Headers.ContentDisposition = $stringHeader
        $multipartContent.Add($stringContent)

        $multipartFile = $file
        $FileStream = [System.IO.FileStream]::new($multipartFile, [System.IO.FileMode]::Open)
        $fileHeader = [System.Net.Http.Headers.ContentDispositionHeaderValue]::new("form-data")
        $fileHeader.Name = "file"
        $fileHeader.FileName = $file
        $fileContent = [System.Net.Http.StreamContent]::new($FileStream)
        $fileContent.Headers.ContentDisposition = $fileHeader
        $multipartContent.Add($fileContent)

        $body = $multipartContent

        $response = Invoke-RestMethod "$Uri/api/v1/upload/release" -Method 'POST' -Headers $headers -Body $body    
        if ($response.error) {
            Write-Output $response.message
            exit 1
        }

        Write-Output "Success on upload attach $attachTitle!"
        Write-Output "Response details:"
        $response | ConvertTo-Json
    }
    catch {
        Write-Error "Error ao fazer o upload do anexo"
        Write-Error "Detalhes:"
        Write-Error $_
        exit 1
    }
}