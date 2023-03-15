using System;
using Abp;
using Abp.Domain.Uow;

namespace Yoyo.Pro.Extensions
{
    /// <summary>
    /// YoyoPro 扩展工作单元支持 DbContext Type
    /// </summary>
    public static class YoyoProUnitOfWorkDbContextProviderExtensions
    {
        public const string YoyoPro_DBCONTEXT_PROVIDER_NAME = nameof(YoyoPro_DBCONTEXT_PROVIDER_NAME);
        public const string YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD = nameof(YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD);

        /// <summary>
        /// 切换 DbContext 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDisposable ChangeDbContextProviderName(this IActiveUnitOfWork unitOfWork, string name)
        {
            if (unitOfWork.Items.TryGetValue(YoyoPro_DBCONTEXT_PROVIDER_NAME, out var currentName))
            {
                unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD] = currentName;
            }
            else
            {
                unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD] = string.Empty;
            }
            unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME] = name;


            return new DisposeAction(() =>
            {
                unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME] = unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD];
                unitOfWork.Items[YoyoPro_DBCONTEXT_PROVIDER_NAME_OLD] = string.Empty;
            });
        }

        /// <summary>
        /// 获取 DbContext Provider 的 Name
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static string GetDbContextProviderName(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork.Items.TryGetValue(YoyoPro_DBCONTEXT_PROVIDER_NAME, out var currentName))
            {
                return currentName as string;
            }

            return null;
        }
    }

}
