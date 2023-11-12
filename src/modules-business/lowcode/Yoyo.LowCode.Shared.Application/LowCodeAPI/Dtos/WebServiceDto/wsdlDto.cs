// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.WebServiceDto
{
    public class wsdlDto
    {
    }

    public class wsdl_Namespaces_Dto
    {
        public string Name { get; set; }

        public string Namespace { get; set; }
    }

    /// <summary>
    /// web service服务
    /// </summary>
    public class wsdl_service_Dto
    {
        public string serviceName { get; set; }

        public List<wsdl_service_prot_Dto> serviceDetail { get; set; }
    }

    /// <summary>
    /// web service服务 的几种方式 和地址
    /// </summary>
    public class wsdl_service_prot_Dto
    {
        public string BindingName { get; set; }
        public string ProtName { get; set; }
        public string PortPath { get; set; }
    }

    /// <summary>
    /// web service描述服务类
    /// </summary>
    public class wsdl_binding_Dto
    {
        public string BindingName { get; set; }

        public string TypeName { get; set; }

        public List<wsdl_binding_Detail_Dto> bindingDetail { get; set; }
    }

    /// <summary>
    /// web service描述服务具体 方法名称 和请求方式
    /// </summary>
    public class wsdl_binding_Detail_Dto
    {
        public string InterfaceName { get; set; }
        public string soapAction { get; set; }
        public string Verb { get; set; }
    }

    /// <summary>
    /// web service描述服务类
    /// </summary>
    public class wsdl_portType_Dto
    {
        public string TypeName { get; set; }

        public List<wsdl_PortType_Detail_Dto> portTypeDetail { get; set; }
    }

    /// <summary>
    /// web service描述服务具体 方法名称 和请求方式
    /// </summary>
    public class wsdl_PortType_Detail_Dto
    {
        public string InterfaceName { get; set; }
        public string input { get; set; }
        public string outinput { get; set; }
    }

    /// <summary>
    /// web service的Message描述服务类
    /// </summary>
    public class wsdl_Message_Dto
    {
        public string InputName { get; set; }

        public List<wsdl_Message_Detail_Dto> portTypeDetail { get; set; }
    }

    /// <summary>
    /// web service的Message描述服务具体Types
    /// </summary>
    public class wsdl_Message_Detail_Dto
    {
        public string Name { get; set; }
        public string type { get; set; }
        public string element { get; set; }
    }

    /// <summary>
    /// web service的Types
    /// </summary>
    public class wsdl_Types_Dto
    {
        public string PartsName { get; set; }

        public List<wsdl_Types_Detail_Dto> portTypeDetail { get; set; }
    }

    /// <summary>
    /// web service的Types 描述服务具体 字段名称
    /// </summary>
    public class wsdl_Types_Detail_Dto
    {
        public string Name { get; set; }
        public string type { get; set; }
    }

    /// <summary>
    /// web service服务
    /// </summary>
    public class WebSrviceInterface
    {
        public string PortType { get; set; }
        public string InterfaceName { get; set; }
        public string Path { get; set; }
        public string SOAPAction { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public List<Interfacefield> filelds { get; set; }
    }

    public class Interfacefield
    {
        public string FileldName { get; set; }
        public string Type { get; set; }
    }
}
