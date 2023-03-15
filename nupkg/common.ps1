# -------------- nuget.config --------------- 
$NUGET_CONFIG = $env:nexus_nuget_config  # nuget配置


# 路径
$packFolder = (Get-Item -Path "./" -Verbose).FullName   # 当前路径
$rootFolder = Join-Path $packFolder "../"               # 项目根目录
$slnFolder = $rootFolder # sln所在目录





# 更新xml文件的选择路径的InnerText
function UpdateXmlInnerText {
  param (
    [string]$path,
    [string]$xPath,
    [string]$innerText
  )
  if (Test-Path $path) {
    [xml]$content = Get-Content $path
    $content.SelectNodes($xPath)[0].set_InnerText($innerText)
    $content.Save($path)
  }
}

