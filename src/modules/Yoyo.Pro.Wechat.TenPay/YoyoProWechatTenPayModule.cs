using Abp;
using Abp.Modules;
using Senparc.Weixin;
using Senparc.Weixin.TenPay.V2;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro
{
    /// <summary>
    /// YoYo Soft Senparc.WeiXin.TenPay  Module
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class YoyoProWechatTenPayModule : AbpModule
    {
     
    }
}
