<a href="https://github.com/zhenlei520/System.Extension.Core/blob/master/README.zh-cn.md">回到目录</a>

# 存储服务 #
<p align="right"><a href="https://github.com/zhenlei520/System.Extension.Core/tree/master/src/Storage/UCloud/README.md">英文</a></p>

在Nuget包市场中搜索`EInfrastructure.Core、EInfrastructure.Core.AutoFac`，并安装最新版本

### UCloud云存储 ###
在Nuget包市场中搜索`EInfrastructure.Core.UCloud.Storage`，并安装最新版本

在Starup中ConfigureServices中添加AutoFac自动注入，实例为：  
    
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			//UCloud云存储
			services.AddUCloudStorage();
			this._serviceProvider = new AutofacAutoRegister().Build(services,
                (builder) => { });
		}
        
通过控制器注入的方式可直接得到IStorageService，之后直接调用即可
例如：

		public class TestController{
			private readonly IStorageService _storageService;
			public TestController(IStorageService storageService){
				_storageService=storageService;
			}

			public void Check()
			{
				_storageService.UploadStream(new UploadByStreamParam("文件key","文件Stream流",null));
			}
		} 

备注：当IStorageService有多个实现类，会导致引用可能不明确，如希望指定实现类：

		public class TestController{
			private readonly IStorageService _storageService;
			public TestController(ICollection<IStorageService> storageService){
				_storageService=storageService.FirstOrDefault(x => x.GetIdentify() == "EInfrastructure.Core.UCloud.Storage");
			}

			public void Check()
			{
				_storageService.UploadStream(new UploadByStreamParam("文件key","文件Stream流",null));
			}
		} 