// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using Masuit.Tools.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.Dtos.WebServiceDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class WebServiceAppService : LowCodeSharedAppServiceBase, IWebServiceAppService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IApplyManager _applyManager;

        public WebServiceAppService(IApplyManager applyManager,
            IWebHostEnvironment hostEnvironment)
        {
            _applyManager = applyManager;
            _environment = hostEnvironment;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="wsdlUrl"></param>
        /// <returns></returns>
        public async Task<List<WebSrviceInterface>> GetWsdl(string wsdlUrl)
        {
            wsdlDto wsdlDto = new wsdlDto();
            try
            {
                //获取wsdl配置
                ServiceDescription serviceDescription = new ServiceDescription();
                using (WebClient client = new WebClient())
                {
                    // 下载 WSDL 文件内容
                    string wsdlContent = client.DownloadString(wsdlUrl);

                    // 将 WSDL 文件内容转换为 Stream 对象
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(wsdlContent)))
                    {
                        // 使用 ServiceDescription 类读取 WSDL 文件内容
                        serviceDescription = ServiceDescription.Read(stream);
                    }
                }

                //服务NamesPaces
                List<wsdl_Namespaces_Dto> namespaces_Dtos = new List<wsdl_Namespaces_Dto>();
                foreach (var item in serviceDescription.Namespaces.ToArray())
                {
                    namespaces_Dtos.Add(new wsdl_Namespaces_Dto
                    {
                        Name = item.Name,
                        Namespace = item.Namespace
                    });
                }

                //服务名称  服务地址  服务支持类型
                List<wsdl_service_Dto> Listservice = new List<wsdl_service_Dto>();
                foreach (Service service in serviceDescription.Services)
                {
                    wsdl_service_Dto service_Dto = new wsdl_service_Dto();
                    service_Dto.serviceDetail = new List<wsdl_service_prot_Dto>();
                    service_Dto.serviceName = service.Name;
                    foreach (Port port in service.Ports)
                    {
                        wsdl_service_prot_Dto _Prot_Dto = new wsdl_service_prot_Dto();
                        _Prot_Dto.ProtName = port.Name;
                        _Prot_Dto.BindingName = port.Binding.Name;
                        //找到请求地址路径
                        if (port.Extensions?.Count > 0)
                        {
                            _Prot_Dto.PortPath = TryGetField(port.Extensions[0], "Location");
                        }
                        service_Dto.serviceDetail.Add(_Prot_Dto);
                    }
                    Listservice.Add(service_Dto);
                }

                //找到服务下面所有方法
                List<wsdl_binding_Dto> Listbinding = new List<wsdl_binding_Dto>();
                foreach (Binding binding in serviceDescription.Bindings)
                {
                    wsdl_binding_Dto binding_Dto = new wsdl_binding_Dto();
                    binding_Dto.bindingDetail = new List<wsdl_binding_Detail_Dto>();
                    binding_Dto.BindingName = binding.Name;
                    binding_Dto.TypeName = binding.Type.Name;
                    foreach (OperationBinding item in binding.Operations)
                    {
                        wsdl_binding_Detail_Dto _Binding_Dto = new wsdl_binding_Detail_Dto();
                        _Binding_Dto.InterfaceName = item.Name;
                        if (item.Extensions.Count > 0)
                        {
                            _Binding_Dto.soapAction = item.Extensions.Count > 0 ? TryGetField(item.Extensions[0], "soapAction") : string.Empty;
                        }
                        if (binding.Extensions.Count > 0)
                        {
                            _Binding_Dto.Verb = binding.Extensions.Count > 0 ? TryGetField(binding.Extensions[0], "Verb") : string.Empty;
                        }
                        binding_Dto.bindingDetail.Add(_Binding_Dto);
                    }
                    Listbinding.Add(binding_Dto);
                }

                //找到方法里面 入参 出参
                List<wsdl_portType_Dto> List_portTypes = new List<wsdl_portType_Dto>();
                foreach (PortType portType in serviceDescription.PortTypes)
                {
                    wsdl_portType_Dto portType_Dto = new wsdl_portType_Dto();
                    portType_Dto.portTypeDetail = new List<wsdl_PortType_Detail_Dto>();
                    portType_Dto.TypeName = portType.Name;
                    foreach (Operation operation in portType.Operations)
                    {
                        wsdl_PortType_Detail_Dto portType_Detail = new wsdl_PortType_Detail_Dto();
                        portType_Detail.InterfaceName = operation.Name;
                        portType_Detail.input = operation.Messages.Input.Message.Name;
                        portType_Detail.outinput = operation.Messages.Output.Message.Name;
                        portType_Dto.portTypeDetail.Add(portType_Detail);
                    }
                    List_portTypes.Add(portType_Dto);
                }

                //找到message里面
                List<wsdl_Message_Dto> List_Messages = new List<wsdl_Message_Dto>();
                foreach (Message message in serviceDescription.Messages)
                {
                    wsdl_Message_Dto wsdl_Message = new wsdl_Message_Dto();
                    wsdl_Message.InputName = message.Name;
                    wsdl_Message.portTypeDetail = new List<wsdl_Message_Detail_Dto>();
                    foreach (MessagePart item in message.Parts)
                    {
                        wsdl_Message_Detail_Dto message_Detail_Dto = new wsdl_Message_Detail_Dto();
                        message_Detail_Dto.Name = item.Name;
                        message_Detail_Dto.type = item.Type.ToString();
                        message_Detail_Dto.element = item.Element?.Name;
                        wsdl_Message.portTypeDetail.Add(message_Detail_Dto);
                    }
                    List_Messages.Add(wsdl_Message);
                }

                //找到types 下的element
                List<wsdl_Types_Dto> ListType = new List<wsdl_Types_Dto>();
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                foreach (XmlSchema schema in serviceDescription.Types.Schemas)
                {
                    schemaSet.Add(schema);
                }
                foreach (XmlSchema schema in schemaSet.Schemas())
                {
                    foreach (XmlSchemaElement element in schema.Elements.Values)
                    {
                        wsdl_Types_Dto wsdl_Types = new wsdl_Types_Dto();
                        wsdl_Types.portTypeDetail = new List<wsdl_Types_Detail_Dto>();
                        wsdl_Types.PartsName = element.Name;
                        if (element.SchemaType == null)
                        {
                        }
                        else
                        {
                            var items = element.SchemaType.GetField<XmlSchemaSequence>("Particle");
                            if (items != null)
                            {
                                foreach (object item in items?.Items)
                                {
                                    wsdl_Types_Detail_Dto Types_Detio = new wsdl_Types_Detail_Dto();
                                    Types_Detio.Name = TryGetField(item, "Name");
                                    var sctype = TryGetFieldToObject(item, "SchemaTypeName");
                                    if (sctype != null)
                                    {
                                        Types_Detio.type = TryGetField(sctype, "Name");
                                    }
                                    wsdl_Types.portTypeDetail.Add(Types_Detio);
                                }
                            }
                        }
                        ListType.Add(wsdl_Types);
                    }
                }

                List<WebSrviceInterface> interfaces = new List<WebSrviceInterface>();
                foreach (var servie in Listservice)
                {
                    foreach (var servieType in servie.serviceDetail)
                    {
                        if (servieType.ProtName.Contains("Soap12"))
                        {
                        }
                        else if (servieType.ProtName.Contains("HttpGet"))
                        {
                        }
                        else if (servieType.ProtName.Contains("HttpPost"))
                        {
                        }
                        else
                        {
                            //所有方法
                            var bindings = Listbinding.Where(t => t.BindingName == servieType.BindingName).FirstOrDefault();
                            var portTypes = List_portTypes.Where(t => t.TypeName == bindings.TypeName).FirstOrDefault();
                            foreach (var bdetail in bindings.bindingDetail)
                            {
                                var portDetail = portTypes.portTypeDetail.Where(t => t.InterfaceName == bdetail.InterfaceName).FirstOrDefault();
                                var input = List_Messages.Where(t => t.InputName == portDetail.input).FirstOrDefault();
                                var outinput = List_Messages.Where(t => t.InputName == portDetail.outinput).FirstOrDefault();

                                string s = string.Empty;
                                string soap = string.Empty;
                                string tns = string.Empty;
                                if (namespaces_Dtos.Where(t => t.Name == "s").FirstOrDefault() != null)
                                {
                                    s = namespaces_Dtos.Where(t => t.Name == "s").FirstOrDefault()?.Namespace;
                                }
                                else
                                {
                                    s = namespaces_Dtos.Where(t => t.Name == "xsd").FirstOrDefault()?.Namespace;
                                    s = s == null ? "配置文件不符合格式，需要手动设置" : s;
                                }
                                if (namespaces_Dtos.Where(t => t.Name == "soap").FirstOrDefault() != null)
                                {
                                    soap = namespaces_Dtos.Where(t => t.Name == "soap").FirstOrDefault()?.Namespace?.Replace("/wsdl", "");
                                }
                                else
                                {
                                    soap = s;
                                }
                                if (namespaces_Dtos.Where(t => t.Name == "tns").FirstOrDefault() != null)
                                {
                                    tns = namespaces_Dtos.Where(t => t.Name == "tns").FirstOrDefault()?.Namespace?.Replace("/wsdl", "");
                                }
                                else
                                {
                                    tns = s;
                                }
                                StringBuilder str = new StringBuilder();
                                str.Append($"<soap:Envelope xmlns:xsi=\"{s}-instance\" xmlns:xsd=\"{s}\" xmlns:soap=\"{soap}envelope/\">");
                                str.Append($"<soap:Body>");
                                str.Append($"<{bdetail.InterfaceName} xmlns=\"{tns}\">");
                                List<Interfacefield> fields = new List<Interfacefield>();
                                foreach (var input_types in input.portTypeDetail)
                                {
                                    if (input_types.Name == "parameters" && input_types.type == "")
                                    {
                                        var types = ListType.Where(t => t.PartsName == input_types.element).FirstOrDefault().portTypeDetail;
                                        foreach (var request in types)
                                        {
                                            str.Append($"<{request.Name}>@{request.Name}</{request.Name}>");
                                            Interfacefield interfacefield = new Interfacefield();
                                            interfacefield.FileldName = request.Name;
                                            interfacefield.Type = request.type;
                                            fields.Add(interfacefield);
                                        }
                                    }
                                }
                                str.Append($"</{bdetail.InterfaceName}>");
                                str.Append($"</soap:Body>");
                                str.Append($"</soap:Envelope>");

                                interfaces.Add(new WebSrviceInterface
                                {
                                    PortType = servieType.ProtName,
                                    InterfaceName = portDetail.InterfaceName,
                                    Path = servieType.PortPath,
                                    SOAPAction = bdetail.soapAction,
                                    RequestBody = str.ToString(),
                                    ResponseBody = "",
                                    filelds = fields
                                });
                            }
                        }
                    }
                }

                var ss = CallWebService(interfaces[0].Path, interfaces[0].SOAPAction
                    , interfaces[0].RequestBody);

                return interfaces;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<WebSrviceInterface>> GetWbeServiceWsdl(Guid ApplyID)
        {
            string wsdlUrl = string.Empty;
            var apply = await _applyManager.QueryAsNoTracking.Where(t => t.Id == ApplyID).FirstOrDefaultAsync();
            if (apply != null)
            {
                if (_environment.IsProduction())
                {
                    wsdlUrl = apply.WebServiceUrlProduce;
                }
                else
                {
                    wsdlUrl = apply.WebServiceUrlTest;
                }
                return await GetWsdl(wsdlUrl);
            }
            return null;
        }

        /// <summary>
        /// try从object中获取数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string TryGetField(object obj, string fieldName)
        {
            try
            {
                return obj.GetField<string>(fieldName);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// try从object中获取数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private object TryGetFieldToObject(object obj, string fieldName)
        {
            try
            {
                return obj.GetField<object>(fieldName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string CallWebService(string url, string soapAction, string xmlRequest)
        {
            // Create a request using a URL that can receive a post.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create the SOAP message.
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xmlRequest);

            // Insert SOAP Action header.
            request.Headers.Add("SOAPAction", soapAction);

            // Convert the XML into a byte array.
            byte[] soapBytes = Encoding.UTF8.GetBytes(soapEnvelopeXml.OuterXml);

            // Set the content type of the request.
            request.ContentType = "text/xml;charset=UTF-8";

            // Set the content length of the request.
            request.ContentLength = soapBytes.Length;

            // Write the XML to the request stream.
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(soapBytes, 0, soapBytes.Length);
            }

            // Get the response.
            using (WebResponse response = request.GetResponse())
            {
                // Get the response stream.
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Read the whole response as a string.
                    string responseString = reader.ReadToEnd();

                    // Return the response string.
                    return responseString;
                }
            }
        }
    }
}
