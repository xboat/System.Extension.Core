﻿using EInfrastructure.Core.HelpCommon;
using EInfrastructure.Core.Interface.IOC;
using EInfrastructure.Core.Interface.Storage;
using EInfrastructure.Core.Interface.Storage.Param.Pictures;
using EInfrastructure.Core.QiNiu.Storage.Config;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace EInfrastructure.Core.QiNiu.Storage
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class PictureService : BaseStorageProvider, IPictureService, ISingleInstance
    {
        /// <summary>
        /// 图片服务
        /// </summary>
        public PictureService(QiNiuStorageConfig qiNiuConfig) : base(qiNiuConfig)
        {
        }

        #region 根据图片base64上传

        /// <summary>
        /// 根据图片base64上传
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Upload(UploadByBase64Param param)
        {
            SetPutPolicy(param.ImgPersistentOps.Key, param.ImgPersistentOps.IsAllowOverlap,
                "");
            string token = Auth.CreateUploadToken(Mac, PutPolicy.ToJsonString());
            FormUploader target = new FormUploader(GetConfig());
            HttpResult result =
                target.UploadData(param.Base64.ConvertToByte(), param.ImgPersistentOps.Key, token,
                    GetPutExtra());
            return result.Code == (int) HttpCode.OK;
        }

        #endregion
        
        #region 抓取资源到空间

        /// <summary>
        /// 抓取资源到空间
        /// </summary>
        /// <param name="fetchFileParam">资源信息</param>
        /// <returns></returns>
        public bool FetchFile(FetchFileParam fetchFileParam)
        {
            FetchResult ret = GetBucketManager()
                .Fetch(fetchFileParam.SourceFileKey, QiNiuConfig.Bucket, fetchFileParam.Key);
            switch (ret.Code)
            {
                case (int) HttpCode.OK:
                case (int) HttpCode.CALLBACK_FAILED:
                    LogCommon.Info($"上传code为：{ret.Code}");
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}