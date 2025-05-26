# Đặt tên cho file solution
$solutionName = "Master.sln"

# Nếu file solution đã tồn tại, xóa nó
if (Test-Path $solutionName) {
    Remove-Item $solutionName
}

# Tạo solution mới
dotnet new sln -n "Master"

# Tìm tất cả các project .csproj trong thư mục con và add vào solution
Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $fullPath = $_.FullName
    Write-Host "Adding project: $fullPath"
    dotnet sln $solutionName add "$fullPath"
}