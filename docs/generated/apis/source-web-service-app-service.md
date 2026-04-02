# WebServiceAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/WebServiceAppService.cs`

## 服务别名

- `IWebServiceAppService`
- `WebServiceAppService`

## 方法列表

- `Task<List<WebSrviceInterface>> GetWsdl(string wsdlUrl)`

- `Task<List<WebSrviceInterface>> GetWbeServiceWsdl(Guid ApplyID)`

- `public WebServiceAppService(IApplyManager applyManager, IWebHostEnvironment hostEnvironment)`

- `return await GetWsdl(wsdlUrl)`

- `private string TryGetField(object obj, string fieldName)`

- `private object TryGetFieldToObject(object obj, string fieldName)`

- `private string CallWebService(string url, string soapAction, string xmlRequest)`
