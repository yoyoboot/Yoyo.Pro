# 执行公用脚本
. ".\common.ps1"

# 切换到源码目录
Set-Location $rootFolder

# 修改NuGet.config
$nugetConfigPath = Join-Path $rootFolder "./NuGet.Config"
Write-Output $NUGET_CONFIG > $nugetConfigPath


# 切换到项目目录
Set-Location $slnFolder

# 修改abpersion.props文件
$abpversionPath = Join-Path $slnFolder "./abpversion.props"
UpdateXmlInnerText -path $abpversionPath -xPath "Project/PropertyGroup/AbpRefMode" -innerText "nuget"

# 切换到脚本启动目录
Set-Location $packFolder


# 执行错误判断
if($Error.Count -eq 0){
    exit 0
}else {
    exit 1
}
