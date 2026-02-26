// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Yoyo.Pro.RequestData
{
    /// <summary>
    /// 通知类型
    /// </summary>
    public enum PushChannelType
    {
        [Description("RobotFeishu")]
        RobotFeishu,
        [Description("RobotWechat")]
        RobotWechat,
        [Description("RobotDingding")]
        RobotDingding,
        [Description("SmsAliyun")]
        SmsAliyun,
        [Description("SmsTenxunyun")]
        SmsTenxunyun,
        [Description("SmsHuaweiyun")]
        SmsHuaweiyun,
        [Description("Email")]
        Email,
        [Description("Firebase")]
        Firebase,
        [Description("RobotDingdingQY")]
        RobotDingdingQY,
    }
}
