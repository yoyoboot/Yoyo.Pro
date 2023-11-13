// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.TestInterface.Dtos;

namespace Yoyo.LowCode.TestInterface
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public class TestInterfaceAppService : LowCodeSharedAppServiceBase
    {
        private static List<DevicegroupsListDto> DevicegroupsList;
        private static List<DquipmentListDto> DquipmentList;
        private static readonly string[] UuidList = new string[] { "a1412b9c", "58830a7c", "bc7f8054" };
        private static readonly string[] ChildUuidList = new string[] { "c1412b9c", "58830c7c", "cc7f8054" };

        //private readonly IRepository<WZPTEST, Guid> _wzpTestRepository;

        private readonly IDatabaseManager _databaseManager;

        /*        public TestInterfaceAppService(IRepository<WZPTEST, Guid> wzpTestRepository, IDatabaseManager databaseManager)
                {
                    _wzpTestRepository = wzpTestRepository;
                    _databaseManager = databaseManager;
                }
        */

        /// <summary>
        /// 设备组
        /// </summary>
        /// <returns></returns>
        public List<DevicegroupsListDto> GetDeviceGroups()
        {
            DevicegroupsList = new List<DevicegroupsListDto>();
            DquipmentList = new List<DquipmentListDto>();
            string[] Groups = new string[] { "A设备组", "B设备组", "C设备组" };
            for (int i = 0; i < Groups.Length; i++)
            {
                DevicegroupsListDto item = new DevicegroupsListDto();
                item.name = Groups[i];
                item.uuid = UuidList[i];
                DevicegroupsList.Add(item);
            }
            DevicegroupsList.ForEach(item =>
            {
                for (int i = 0; i < 3; i++)
                {
                    DquipmentListDto items = new DquipmentListDto();
                    items.DevicegroupsId = item.uuid;
                    items.uuid = ChildUuidList[i];
                    items.name = item.name + "综合测试设备" + i + 1;
                    DquipmentList.Add(items);
                }
            });
            return DevicegroupsList;
        }

        /// <summary>
        /// 设备
        /// </summary>
        /// <returns></returns>
        public List<DquipmentListDto> GetDquipments()
        {
            return DquipmentList;
        }

        /// <summary>
        /// 查找设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DquipmentListDto> GetDquipmentList(string id)
        {
            if (DquipmentList == null)
            {
                return null;
            }
            var list = DquipmentList.FindAll(x => x.DevicegroupsId == id);
            return list;
        }

        [HttpGet]
        public object TestGet()
        {
            var data = new
            {
                Name = AbpSession.GetUserName(),
                TheCurrentTime = DateTime.Now,
                Fraction = new Random().Next(1, 100),
                IdNumber = Guid.NewGuid().ToString(),
                Chain = new Random().Next(1, 100),
            };
            return data;
        }

        [HttpPost]
        public object TestPost()
        {
            var data = new
            {
                Name = AbpSession.GetUserName(),
                TheCurrentTime = DateTime.Now,
                Fraction = new Random().Next(1, 100),
                IdNumber = Guid.NewGuid().ToString(),
                Chain = Guid.NewGuid().ToString(),
            };
            return data;
        }

        [HttpPost]
        public List<TestRdoDto> GetRdoCombox()
        {
            string[] Groups = new string[] { "检验单", "针管", "注射器" };
            List<TestRdoDto> list = new List<TestRdoDto>();

            foreach (var item in Groups)
            {
                TestRdoDto rdoDto = new TestRdoDto();
                rdoDto.Id = Guid.Parse("25533136-9AF7-4D24-72FC-08DABE2EB2BE");
                rdoDto.Name = item;
                rdoDto.CreateTime = DateTime.Now;
                rdoDto.revOfRcdId = Guid.Parse("35533136-9AF7-4D24-72FC-08DABE2EB2BE");
                for (int i = 0; i < 1; i++)
                {
                    RdoList rdoLists = new RdoList();
                    rdoLists.Id = Guid.Parse("32533136-9AF7-4D24-72FC-08DABE2EB2BE");
                    rdoLists.revision = new Random().Next(3) + "." + new Random().Next(10);
                    rdoLists.CreateTime = DateTime.Now;
                    rdoLists.baseId = rdoDto.Id;
                    rdoDto.rdoList ??= new List<RdoList>();
                    rdoDto.rdoList.Add(rdoLists);
                }
                list.Add(rdoDto);
            }
            return list;
        }

        /*        public void InsertTest(WZPTEST wZPTEST)
                {
                    var dc = new Dictionary<string, object>();
                    dc.Add("ID", "25C89FC7D7E2A6478A91E8FD4D5686FC");
                    dc.Add("KSignature",wZPTEST.KSignature);

                    var db = _databaseManager.GetDatabase(null);

                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        var dasdfs = "dssdf";
                    };

                    db.Insertable(dc).AS("WZPTEST").ExecuteCommand();
                }*/
    }
}
